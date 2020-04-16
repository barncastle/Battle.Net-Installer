using System;
using System.Threading.Tasks;
using CommandLine;

namespace BNetInstaller
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using var parser = new Parser(s =>
            {
                s.HelpWriter = Console.Error;
                s.CaseInsensitiveEnumValues = true;
                s.AutoVersion = false;
            });

            var options = parser.ParseArguments<Options>(args);
            options.MapResult(async o => await Run(o), e => Task.FromResult(0)).Wait();
        }

        private static async Task Run(Options options)
        {
            using var app = new AgentApp();
            options.Sanitise();
            string locale = options.Locale.ToString();

            Console.WriteLine("Authenticating");
            await app.AgentEndpoint.Get();

            Console.WriteLine("Queuing install");
            app.InstallEndpoint.Model.InstructionsDataset = new[] { "torrent", "win", options.Product, locale.ToLowerInvariant() };
            app.InstallEndpoint.Model.InstructionsPatchUrl = $"http://us.patch.battle.net:1119/{options.Product}";
            app.InstallEndpoint.Model.Uid = options.UID;
            await app.InstallEndpoint.Post();

            Console.WriteLine("Starting install");
            app.InstallEndpoint.Product.Model.GameDir = options.Directory;
            app.InstallEndpoint.Product.Model.Language[0] = locale;
            app.InstallEndpoint.Product.Model.SelectedAssetLocale = locale;
            app.InstallEndpoint.Product.Model.SelectedLocale = locale;
            await app.InstallEndpoint.Product.Post();

            app.UpdateEndpoint.Model.Uid = options.UID;
            await app.UpdateEndpoint.Post();

            // process the install
            Console.WriteLine();
            await ProgressLoop(options, app);

            // send close signal
            await app.AgentEndpoint.Delete();
        }

        private static async Task ProgressLoop(Options options, AgentApp app)
        {
            var locale = options.Locale.ToString();
            var cursorLeft = Console.CursorLeft;
            var cursorTop = Console.CursorTop;

            var endpoint = app.InstallEndpoint.Product;
            var updating = false;

            while (true)
            {
                var stats = await endpoint.Get();

                // check for completion
                var complete = stats.Value<bool?>("download_complete");
                if (complete == true)
                    break;

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
                        break;
                }
                else if (!updating)
                {
                    // try the update endpoint instead
                    endpoint = app.UpdateEndpoint.Product;
                    updating = true;
                }
                else
                {
                    // another agent or the BNet app has taken control of this install
                    Console.WriteLine("Another application has taken over. Launch the Battle.Net app to resume installation.");
                    break;
                }
            }
        }

        private static void Print(string label, string value)
        {
            Console.Write(label.PadRight(20, ' '));
            Console.WriteLine(value);
        }
    }
}
