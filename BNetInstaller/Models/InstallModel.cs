namespace BNetInstaller.Models;

internal sealed class InstallModel : ProductPriorityModel
{
    public string InstructionsPatchUrl { get; set; }
    public string InstructionsProduct { get; set; } = "NGDP";
    public double MonitorPid { get; set; } = 12345;
}
