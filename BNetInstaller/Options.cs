using System;
using System.Text.RegularExpressions;
using BNetInstaller.Constants;
using CommandLine;

namespace BNetInstaller
{
    internal class Options
    {
        [Option("prod", Required = true, HelpText = "TACT Product")]
        public string Product { get; set; }

        [Option("lang", Required = true, HelpText = "Game/Asset language")]
        public Locale Locale { get; set; }

        [Option("dir", Required = true, HelpText = "Installation Directory")]
        public string Directory { get; set; }

        [Option("uid", HelpText = "Agent Product UID (Required if different to the TACT product)")]
        public string UID { get; set; }

        [Option("repair", HelpText = "Run installation repair")]
        public bool Repair { get; set; }

        public void Sanitise()
        {
            // ensure a UID exists
            if (string.IsNullOrWhiteSpace(UID))
                UID = Product;
            // remove _locale suffix for wiki copy-pasters
            if (UID.Contains("_locale", StringComparison.OrdinalIgnoreCase))
                UID = Regex.Replace(UID, "\\(?_locale\\)?", $"_{Locale}", RegexOptions.IgnoreCase);

            Product = Product.ToLowerInvariant().Trim();
            UID = UID.ToLowerInvariant().Trim();
            Directory = Directory.Replace("/", "\\").Trim().TrimEnd('\\') + '\\';
        }

        public static string[] Generate()
        {
            var args = new string[9];

            Console.WriteLine("Please complete the following information:");

            static string GetInput() => Console.ReadLine().Trim().Trim('"');

            Console.Write("TACT Product: ");
            args[0] = "--prod";
            args[1] = GetInput();

            Console.Write("Agent UID: ");
            args[2] = "--uid";
            args[3] = GetInput();

            Console.Write("Installation Directory: ");
            args[4] = "--dir";
            args[5] = GetInput();

            Console.Write("Game/Asset Language: ");
            args[6] = "--lang";
            args[7] = GetInput();

            Console.Write("Repair Install (Y/N): ");
            args[8] = GetInput().ToUpper() == "Y" ? "--repair" : "";

            Console.WriteLine();

            return args;
        }
    }
}
