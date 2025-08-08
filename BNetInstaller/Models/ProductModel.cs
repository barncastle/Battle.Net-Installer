namespace BNetInstaller.Models;

internal sealed class ProductModel : IModel
{
    public string AccountCountry { get; set; } = "USA";
    public bool Finalized { get; set; } = true;
    public string GameDir { get; set; }
    public string GeoIpCountry { get; set; } = "US";
    public string[] Language { get; set; } = ["enus"];
    public string SelectedAssetLocale { get; set; } = "enus";
    public string SelectedLocale { get; set; } = "enus";
    public string Shortcut { get; set; } = "all";
    public string TomeTorrent { get; set; } = "";
}
