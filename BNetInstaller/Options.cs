using System;
using System.Text.RegularExpressions;
using BNetInstaller.Constants;
using CommandLine;

namespace BNetInstaller
{
    class Options
    {
        [Option("prod", Required = true, HelpText = "TACT Product")]
        public string Product { get; set; }

        [Option("lang", Required = true, HelpText = "Game/Asset language")]
        public Locale Locale { get; set; }

        [Option("dir", Required = true, HelpText = "Installation Directory")]
        public string Directory { get; set; }

        [Option("uid", HelpText = "Agent Product UID (Required if different to the TACT product)")]
        public string UID { get; set; }

        public void Santise()
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
    }
}
