using System;
using System.Threading.Tasks;
using BNetInstaller.Constants;
using BNetInstaller.Endpoints;
using CommandLine;

namespace BNetInstaller
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using var parser = new Parser(s =>
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
            using var app = new AgentApp();
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
                _ => throw new NotImplementedException(),
            };

            // process the task
            await operation;

            // send close signal
            await app.AgentEndpoint.Delete();
        }

        private static async Task InstallProduct(Options options, AgentApp app)
        {
            // initiate download
            app.UpdateEndpoint.Model.Uid = options.UID;
            await app.UpdateEndpoint.Post();

            // first try install endpoint
            if (await ProgressLoop(options, app.InstallEndpoint.Product))
                return;

            // then try the update endpoint instead
            if (await ProgressLoop(options, app.UpdateEndpoint.Product))
                return;

            // failing that another agent or the BNet app has probably taken control of the install
            Console.WriteLine("Another application has taken over. Launch the Battle.Net app to resume installation.");
        }

        private static async Task RepairProduct(Options options, AgentApp app)
        {
            // initiate repair
            app.RepairEndpoint.Model.Uid = options.UID;
            await app.RepairEndpoint.Post();

            // run the repair endpoint
            if (await ProgressLoop(options, app.RepairEndpoint.Product))
                return;

            Console.WriteLine("Unable to repair this product.");
        }

        private static async Task<bool> ProgressLoop(Options options, ProductEndpoint endpoint)
        {
            var locale = options.Locale.ToString();
            var cursorLeft = Console.CursorLeft;
            var cursorTop = Console.CursorTop;

            static void Print(string label, string value)
            {
                Console.Write(label.PadRight(20, ' '));
                Console.WriteLine(value);
            }

            while (true)
            {
                var stats = await endpoint.Get();

                // check for completion
                var complete = stats.Value<bool?>("download_complete");
                if (complete == true)
                    return true;

                // get progress percentage
                var progress = stats.Value<float?>("progress");
                if (progress.HasValue)
                {
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Print("Downloading:", options.Product);
                    Print("Language:", locale);
                    Print("Directory:", options.Directory);
                    Print("Progress:", progress.Value.ToString("P4"));
                    await Task.Delay(2000);

                    // exit @ 100%
                    if (progress == 1f)
                        return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
