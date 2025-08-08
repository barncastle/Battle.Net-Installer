namespace BNetInstaller.Models;

internal sealed class PriorityModel : IModel
{
    public bool InsertAtHead { get; set; } = true;
    public double Value { get; set; } = 900;
}
