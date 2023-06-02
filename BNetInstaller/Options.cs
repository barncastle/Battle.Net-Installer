using System.Text.RegularExpressions;
using BNetInstaller.Constants;
using CommandLine;

namespace BNetInstaller;

internal sealed partial class Options
{
    [Option("prod", Required = true, HelpText = "TACT Product")]
    public string Product { get; set; }

    [Option("lang", Required = true, HelpText = "Game/Asset Language")]
    public Locale Locale { get; set; }

    [Option("dir", Required = true, HelpText = "Installation Directory")]
    public string Directory { get; set; }

    [Option("uid", HelpText = "Agent Product UID (Required if different to the TACT product)")]
    public string UID { get; set; }

    [Option("repair", HelpText = "Repair Product Installation")]
    public bool Repair { get; set; }

    [Option("console-env", Hidden = true)]
    public bool ConsoleEnvironment { get; set; } = true;

    [Option("post-download-script", Hidden = true)]
    public string PostDownloadScript { get; set; }

    public void Sanitise()
    {
        // ensure a UID exists
        if (string.IsNullOrWhiteSpace(UID))
            UID = Product;

        // remove _locale suffix for wiki copy-pasters
        if (UID.Contains("_locale", StringComparison.OrdinalIgnoreCase))
            UID = LocaleSuffixRegex().Replace(UID, $"_{Locale}");

        Product = Product.ToLowerInvariant().Trim();
        UID = UID.ToLowerInvariant().Trim();
        Directory = Directory.Replace("/", "\\").Trim().TrimEnd('\\') + '\\';
    }

    public static string[] Create()
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
        if (args[8] is ['Y', ..])
            args[8] = "--repair";

        return args;
    }

    [GeneratedRegex("\\(?_locale\\)?", RegexOptions.IgnoreCase, "en-GB")]
    private static partial Regex LocaleSuffixRegex();
}
