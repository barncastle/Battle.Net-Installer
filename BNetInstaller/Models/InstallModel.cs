namespace BNetInstaller.Models
{
    class InstallModel : ProductPriorityModel
    {
        public string[] InstructionsDataset { get; set; }
        public string InstructionsPatchUrl { get; set; }
        public string InstructionsProduct { get; set; } = "NGDP";
        public double MonitorPid { get; set; } = 12345;
    }
}
