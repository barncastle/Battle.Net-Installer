using System.Diagnostics;
using BNetInstaller.Constants;
using BNetInstaller.Endpoints;
using CommandLine;

namespace BNetInstaller;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        if (args is not { Length: > 0 })
            args = Options.Create();

        using Parser parser = new(s =>
        {
            s.HelpWriter = Console.Error;
            s.CaseInsensitiveEnumValues = true;
            s.AutoVersion = false;
        });

        await parser
            .ParseArguments<Options>(args)
            .MapResult(Run, Task.FromResult);
    }

    private static async Task Run(Options options)
    {
        using AgentApp app = new();
        options.Sanitise();

        var locale = options.Locale.ToString();
        var mode = options.Repair ? Mode.Repair : Mode.Install;

        Console.WriteLine("Authenticating");
        await app.AgentEndpoint.Get();

        Console.WriteLine($"Queuing {mode}");
        app.InstallEndpoint.Model.InstructionsDataset = new[] { "torrent", "win", options.Product, locale.ToLowerInvariant() };
        app.InstallEndpoint.Model.InstructionsPatchUrl = $"http://us.patch.battle.net:1119/{options.Product}";
        app.InstallEndpoint.Model.Uid = options.UID;
        await app.InstallEndpoint.Post();

        Console.WriteLine($"Starting {mode}");
        app.InstallEndpoint.Product.Model.GameDir = options.Directory;
        app.InstallEndpoint.Product.Model.Language[0] = locale;
        app.InstallEndpoint.Product.Model.SelectedAssetLocale = locale;
        app.InstallEndpoint.Product.Model.SelectedLocale = locale;
        await app.InstallEndpoint.Product.Post();

        Console.WriteLine();

        var operation = mode switch
        {
            Mode.Install => InstallProduct(options, app),
            Mode.Repair => RepairProduct(options, app),
            _ => throw new NotSupportedException(),
        };

        // process the task
        var complete = await operation;

        // send close signal
        await app.AgentEndpoint.Delete();

        // run the post download script if applicable
        RunPostDownloadScript(options, complete);
    }

    private static async Task<bool> InstallProduct(Options options, AgentApp app)
    {
        // initiate download
        app.UpdateEndpoint.Model.Uid = options.UID;
        await app.UpdateEndpoint.Post();

        // first try install endpoint
        if (await ProgressLoop(options, app.InstallEndpoint.Product))
            return true;

        // then try the update endpoint instead
        if (await ProgressLoop(options, app.UpdateEndpoint.Product))
            return true;

        // failing that another agent or the BNet app has probably taken control of the install
        Console.WriteLine("Another application has taken over. Launch the Battle.Net app to resume installation.");
        return false;
    }

    private static async Task<bool> RepairProduct(Options options, AgentApp app)
    {
        // initiate repair
        app.RepairEndpoint.Model.Uid = options.UID;
        await app.RepairEndpoint.Post();

        // run the repair endpoint
        if (await ProgressLoop(options, app.RepairEndpoint.Product))
            return true;

        Console.WriteLine("Unable to repair this product.");
        return false;
    }

    private static async Task<bool> ProgressLoop(Options options, ProductEndpoint endpoint)
    {
        var locale = options.Locale.ToString();
        var cursor = (Left: 0, Top: 0);

        if (options.ConsoleEnvironment)
            cursor = Console.GetCursorPosition();

        static void Print(string label, object value) =>
            Console.WriteLine("{0,-20}{1,-20}", label, value);

        while (true)
        {
            var stats = await endpoint.Get();

            // check for completion
            var complete = stats["download_complete"]?.GetValue<bool?>();

            if (complete == true)
                return true;

            // get progress percentage and playability
            var progress = stats["progress"]?.GetValue<float?>();
            var playable = stats["playable"]?.GetValue<bool?>();

            if (!progress.HasValue)
                return false;

            // some non-console environments don't support
            // cursor positioning or line rewriting
            if (options.ConsoleEnvironment)
            {
                Console.SetCursorPosition(cursor.Left, cursor.Top);
                Print("Downloading:", options.Product);
                Print("Language:", locale);
                Print("Directory:", options.Directory);
                Print("Progress:", progress.Value.ToString("P4"));
                Print("Playable:", playable.GetValueOrDefault());
            }
            else
            {
                Print("Progress:", progress.Value.ToString("P4"));
            }

            await Task.Delay(2000);

            // exit @ 100%
            if (progress == 1f)
                return true;
        }
    }

    private static void RunPostDownloadScript(Options options, bool complete)
    {
        if (complete && File.Exists(options.PostDownloadScript))
        Process.Start(options.PostDownloadScript);
    }
}
