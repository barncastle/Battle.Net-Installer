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
    public readonly AgentEndpoint AgentEndpoint;
    public readonly InstallEndpoint InstallEndpoint;
    public readonly UpdateEndpoint UpdateEndpoint;
    public readonly RepairEndpoint RepairEndpoint;
    public readonly VersionEndpoint VersionEndpoint;

    private readonly Process _process;
    private readonly int _port;
    private readonly AgentClient _client;

    public AgentApp()
    {
        if (!StartProcess(out _process, out _port))
        {
            Console.WriteLine("Please ensure Battle.net is installed and has recently been signed in to.");
            Environment.Exit(0);
        }

        _client = new(_port);

        AgentEndpoint = new(_client);
        InstallEndpoint = new(_client);
        UpdateEndpoint = new(_client);
        RepairEndpoint = new(_client);
        VersionEndpoint = new(_client);
    }

    private static bool StartProcess(out Process process, out int port)
    {
        (process, port) = (null, -1);

        var agentPath = GetAgentPath();

        if (!File.Exists(agentPath))
        {
            Console.WriteLine("Unable to find Agent.exe.");
            return false;
        }

        try
        {
            process = Process.Start(new ProcessStartInfo(agentPath)
            {
                Arguments = "--internalclienttools",
                UseShellExecute = true,
            });

            // detect listening port
            while (!process.HasExited && port == -1)
            {
                Thread.Sleep(250);
                port = NativeMethods.GetProcessListeningPort(process.Id);
            }

            if (process.HasExited || port == -1)
            {
                Console.WriteLine("Unable to connect to Agent.exe.");
                return false;
            }

            return true;
        }
        catch (Win32Exception)
        {
            Console.WriteLine("Unable to start Agent.exe.");
            return false;
        }
    }

    private static string GetAgentPath()
    {
        var agentDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Battle.net", "Agent");
        var parentPath = Path.Combine(agentDirectory, "Agent.exe");
        var parentVersion = 0;

        // read parent Agent.exe version
        if (File.Exists(parentPath))
            parentVersion = FileVersionInfo.GetVersionInfo(parentPath).ProductPrivatePart;

        // return expected child Agent path
        return Path.Combine(agentDirectory, $"Agent.{parentVersion}", "Agent.exe");
    }

    public void Dispose()
    {
        if (_process?.HasExited == false)
            _process.Kill();

        _client?.Dispose();
        _process?.Dispose();
    }
}
