using System;
using System.Text.RegularExpressions;
using BNetInstaller.Constants;
//using CommandLine;

namespace BNetInstaller;

internal class Options
{
    //[Option("prod", Required = true, HelpText = "TACT Product")]
    public string Product { get; set; }

    //[Option("lang", Required = true, HelpText = "Game/Asset language")]
    public Locale Locale { get; set; }

    //[Option("dir", Required = true, HelpText = "Installation Directory")]
    public string Directory { get; set; }

    //[Option("uid", HelpText = "Agent Product UID (Required if different to the TACT product)")]
    public string UID { get; set; }

    //[Option("repair", HelpText = "Run installation repair")]
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
        static string GetInput(string message)
        {
            Console.Write(message);
            return Console.ReadLine().Trim().Trim('"');
        }

        Console.WriteLine("Please complete the following information:");

        var args = new string[9]
        {
            "--prod",
            GetInput("TACT Product: "),
            "--uid",
            GetInput("Agent UID: "),
            "--dir",
            GetInput("Installation Directory: "),
            "--lang",
            GetInput("Game/Asset Language: "),
            GetInput("Repair Install (Y/N): ").ToUpper()
        };

        Console.WriteLine();

        // fix repair arg
        if (args[8] != "" && args[8][0] == 'Y')
            args[8] = "--repair";

        return args;
    }
}
