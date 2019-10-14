using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;

namespace BNetInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser(s =>
            {
                s.HelpWriter = Console.Error;
                s.CaseInsensitiveEnumValues = true;
                s.AutoVersion = false;
            });

            var options = parser.ParseArguments<Options>(args);
            options.MapResult(async o => await Run(o), e => Task.FromResult(0)).Wait();

            parser.Dispose();
        }

        static async Task Run(Options options)
        {
            using (var app = new AgentApp())
            {
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
        }

        static async Task ProgressLoop(Options options, AgentApp app)
        {
            string locale = options.Locale.ToString();
            int cursorLeft = Console.CursorLeft, cursorTop = Console.CursorTop;
            bool? complete;
            float? progress;

            while (true)
            {
                var stats = await app.InstallEndpoint.Product.Get();

                // check for completion
                complete = stats.Value<bool?>("download_complete");
                if (complete == true)
                    return;

                // get progress percentage
                progress = stats.Value<float?>("progress");

                if (progress.HasValue)
                {
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Print("Downloading:", options.Product);
                    Print("Language:", locale);
                    Print("Directory:", options.Directory);
                    Print("Progress:", progress.Value.ToString("P4"));
                }
                else
                {
                    // another agent or the BNet app has taken control of this install
                    // this will now show in the BNet app so no worries
                    Console.WriteLine("Another application has taken over. Launch the Battle.Net app to resume installation.");
                    return;
                }

                Thread.Sleep(2000);
            }
        }

        static void Print(string label, string value)
        {
            Console.Write(label.PadRight(20, ' '));
            Console.WriteLine(value);
        }
    }
}
