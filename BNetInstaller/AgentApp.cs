using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using BNetInstaller.Endpoints.Agent;
using BNetInstaller.Endpoints.Game;
using BNetInstaller.Endpoints.Install;
using BNetInstaller.Endpoints.Repair;
using BNetInstaller.Endpoints.Update;
using BNetInstaller.Endpoints.Version;

namespace BNetInstaller
{
    internal class AgentApp : IDisposable
    {
        public readonly AgentEndpoint AgentEndpoint;
        public readonly InstallEndpoint InstallEndpoint;
        public readonly UpdateEndpoint UpdateEndpoint;
        public readonly RepairEndpoint RepairEndpoint;
        public readonly GameEndpoint GameEndpoint;
        public readonly VersionEndpoint VersionEndpoint;

        private readonly string AgentPath;
        private readonly int Port = 5050;

        private Process Process;
        private Requester Requester;

        public AgentApp()
        {
            AgentPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Battle.net", "Agent", "Agent.exe");

            if (!StartProcess())
            {
                Console.WriteLine("Please ensure Battle.net is installed and has recently been opened.");
                Environment.Exit(0);
            }

            AgentEndpoint = new AgentEndpoint(Requester);
            InstallEndpoint = new InstallEndpoint(Requester);
            UpdateEndpoint = new UpdateEndpoint(Requester);
            RepairEndpoint = new RepairEndpoint(Requester);
            GameEndpoint = new GameEndpoint(Requester);
            VersionEndpoint = new VersionEndpoint(Requester);
        }

        private bool StartProcess()
        {
            if (File.Exists(AgentPath))
            {
                try
                {
                    Process = Process.Start(AgentPath, $"--port={Port}");
                    Requester = new Requester(Port);
                    return true;
                }
                catch (Win32Exception)
                {
                    Console.WriteLine("Unable to start Agent.exe.");
                }
            }
            else
            {
                Console.WriteLine("Unable to find Agent.exe.");
            }

            return false;
        }

        public void Dispose()
        {
            if (Process?.HasExited == false)
                Process.Kill();

            Requester?.Dispose();
            Process?.Dispose();
        }
    }
}
