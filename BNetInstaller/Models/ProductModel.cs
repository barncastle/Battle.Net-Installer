namespace BNetInstaller.Models;

internal class ProductModel
{
    public string AccountCountry { get; set; } = "USA";
    public bool Finalized { get; set; } = true;
    public string GameDir { get; set; }
    public string GeoIpCountry { get; set; } = "EU";
    public string[] Language { get; set; } = new[] { "ruru" };
    public string SelectedAssetLocale { get; set; } = "ruru";
    public string SelectedLocale { get; set; } = "ruru";
    public string Shortcut { get; set; } = "all";
    public string TomeTorrent { get; set; } = "";
}
