using System.Diagnostics;
using BNetInstaller.Operations;

namespace BNetInstaller;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        if (args is not { Length: > 0 })
            args = OptionsBinder.CreateArgs();

        await OptionsBinder
            .BuildRootCommand(Run)
            .Parse(args)
            .InvokeAsync();
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
        app.InstallEndpoint.Model.InstructionsDataset = ["torrent", "win", options.Product, locale.ToLowerInvariant()];
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

        AgentTask<bool> operation = mode switch
        {
            Mode.Install => new InstallProductTask(options, app),
            Mode.Repair => new RepairProductTask(options, app),
            _ => throw new NotSupportedException(),
        };

        // process the task
        var complete = await operation;

        // send close signal
        await app.AgentEndpoint.Delete();

        // run the post download app/script if applicable
        if (complete && File.Exists(options.PostDownload))
            Process.Start(options.PostDownload);
    }
}

file enum Mode
{
    Install,
    Repair
}
