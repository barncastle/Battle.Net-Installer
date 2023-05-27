using System.ComponentModel;
using System.Diagnostics;
using BNetInstaller.Endpoints.Agent;
using BNetInstaller.Endpoints.Install;
using BNetInstaller.Endpoints.Repair;
using BNetInstaller.Endpoints.Update;
using BNetInstaller.Endpoints.Version;

namespace BNetInstaller;

internal sealed class AgentApp : IDisposable
{
    public const int Port = 5050;

    public readonly AgentEndpoint AgentEndpoint;
    public readonly InstallEndpoint InstallEndpoint;
    public readonly UpdateEndpoint UpdateEndpoint;
    public readonly RepairEndpoint RepairEndpoint;
    public readonly VersionEndpoint VersionEndpoint;

    private readonly string AgentPath;
    private readonly Process Process;
    private readonly AgentClient Client;

    public AgentApp()
    {
        AgentPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Battle.net", "Agent", "Agent.exe");

        if (!StartProcess(out var process))
        {
            Console.WriteLine("Please ensure Battle.net is installed and has recently been opened.");
            Environment.Exit(0);
        }

        Process = process;
        Client = new(Port);

        AgentEndpoint = new(Client);
        InstallEndpoint = new(Client);
        UpdateEndpoint = new(Client);
        RepairEndpoint = new(Client);
        VersionEndpoint = new(Client);
    }

    private bool StartProcess(out Process process)
    {
        if (!File.Exists(AgentPath))
        {
            process= null;
            Console.WriteLine("Unable to find Agent.exe.");
            return false;
        }

        try
        {
            process = Process.Start(AgentPath, $"--port={Port}");
            return true;
        }
        catch (Win32Exception)
        {
            process = null;
            Console.WriteLine("Unable to start Agent.exe.");
            return false;
        }
    }

    public void Dispose()
    {
        if (Process?.HasExited == false)
            Process.Kill();

        Client?.Dispose();
        Process?.Dispose();
    }
}
