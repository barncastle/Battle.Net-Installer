using System.CommandLine;
using System.Text.RegularExpressions;

namespace BNetInstaller;

internal sealed partial class Options
{
    public string Product { get; set; }
    public Locale Locale { get; set; }
    public string Directory { get; set; }
    public string UID { get; set; }
    public bool Repair { get; set; }
    public bool ConsoleEnvironment { get; set; }
    public string PostDownloadScript { get; set; }

    public void Sanitise()
    {
        // ensure a UID exists
        if (string.IsNullOrWhiteSpace(UID))
            UID = Product;

        // remove _locale suffix for wiki copy-pasters
        if (UID.Contains("_locale", StringComparison.OrdinalIgnoreCase))
            UID = ExtractLocaleRegex().Replace(UID, $"_{Locale}");

        Product = Product.ToLowerInvariant().Trim();
        UID = UID.ToLowerInvariant().Trim();
        Directory = Path.GetFullPath(Directory + "\\");
    }

    [GeneratedRegex("\\(?_locale\\)?", RegexOptions.IgnoreCase)]
    private static partial Regex ExtractLocaleRegex();
}

internal static class OptionsBinder
{
    private static readonly Option<string> Product = new("--prod")
    {
        HelpName = "TACT Product",
        Required = true
    };

    private static readonly Option<Locale> Locale = new("--lang")
    {
        HelpName = "Game/Asset language",
        Required = true
    };

    private static readonly Option<string> Directory = new("--dir")
    {
        HelpName = "Installation Directory",
        Required = true
    };

    private static readonly Option<string> UID = new("--uid")
    {
        HelpName = "Agent Product UID (Required if different to the TACT product)",
        Required = true
    };

    private static readonly Option<bool> Repair = new("--repair")
    {
        HelpName = "Run installation repair"
    };

    private static readonly Option<bool?> ConsoleEnvironment = new("--console-env")
    {
        Hidden = true
    };

    private static readonly Option<string> PostDownloadScript = new("--post-download-script")
    {
        Hidden = true
    };

    public static string[] CreateArgs()
    {
        static string GetInput(string message)
        {
            Console.Write(message);
            return Console.ReadLine()?.Trim().Trim('"');
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

    public static RootCommand BuildRootCommand(Func<Options, Task> task)
    {
        var rootCommand = new RootCommand()
        {
            Product,
            Locale,
            Directory,
            UID,
            Repair
        };

        rootCommand.SetAction(async context =>
        {
            await task(new()
            {
                Product = context.CommandResult.GetValue(Product),
                Locale = context.CommandResult.GetValue(Locale),
                Directory = context.CommandResult.GetValue(Directory),
                UID = context.CommandResult.GetValue(UID),
                Repair = context.CommandResult.GetValue(Repair),
                ConsoleEnvironment = context.CommandResult.GetValue(ConsoleEnvironment) ?? true, // https://github.com/dotnet/command-line-api/issues/2257
                PostDownloadScript = context.CommandResult.GetValue(PostDownloadScript),
            });
        });

        rootCommand.TreatUnmatchedTokensAsErrors = false;

        return rootCommand;
    }
}

internal enum Locale
{
    arSA,
    enSA,
    deDE,
    enUS,
    esMX,
    ptBR,
    esES,
    frFR,
    itIT,
    koKR,
    plPL,
    ruRU,
    zhCN,
    zhTW
}
