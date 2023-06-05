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

    private readonly string _agentPath;
    private readonly Process _process;
    private readonly AgentClient _client;

    public AgentApp()
    {
        _agentPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Battle.net", "Agent", "Agent.exe");

        if (!StartProcess(out var process))
        {
            Console.WriteLine("Please ensure Battle.net is installed and has recently been signed in to.");
            Environment.Exit(0);
        }

        _process = process;
        _client = new(Port);

        AgentEndpoint = new(_client);
        InstallEndpoint = new(_client);
        UpdateEndpoint = new(_client);
        RepairEndpoint = new(_client);
        VersionEndpoint = new(_client);
    }

    private bool StartProcess(out Process process)
    {
        if (!File.Exists(_agentPath))
        {
            process= null;
            Console.WriteLine("Unable to find Agent.exe.");
            return false;
        }

        try
        {
            process = Process.Start(_agentPath, $"--port={Port}");
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
        if (_process?.HasExited == false)
            _process.Kill();

        _client?.Dispose();
        _process?.Dispose();
    }
}
