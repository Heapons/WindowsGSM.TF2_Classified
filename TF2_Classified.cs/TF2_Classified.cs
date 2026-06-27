using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WindowsGSM.Functions;
using WindowsGSM.GameServer.Engine;

namespace WindowsGSM.Plugins
{
    public class TF2_Classified : SteamCMDAgent
    {
        // - Plugin Details
        public Plugin Plugin = new Plugin
        {
            name = "WindowsGSM.TF2_Classified",
            author = "Raziel7893",
            description = "WindowsGSM plugin for supporting Team Fortress 2 Classified Dedicated Server",
            version = "1.1",
            url = "https://github.com/Heapons/WindowsGSM.TF2_Classified",
            color = "#34FFeb"
        };
        // - Plugin Details
        public bool AllowsEmbedConsole = true;
        public int PortIncrements = 1;
        public dynamic QueryMethod = new GameServer.Query.A2S();
        public override bool loginAnonymous => true;
        // - Game server default values
        public string Port = "27015";
        public string QueryPort = "27015";
        public string Maxplayers = "32";

        public string FullName = "Team Fortress 2 Classified Dedicated Server";
        public string Defaultmap { get { return "cp_badlands"; } }
        public string Game { get { return "tf2classified"; } }
        public override string AppId { get { return "3557020"; } }
        public string Additional { get { return "-tickrate 64"; } }

        public override string StartPath => "srcds_win64.exe";

        public TF2_Classified(Functions.ServerConfig serverData): base(serverData)
        {
            base.serverData = serverData;
        }

        public async Task<Process> Start()
        {
            string srcdsPath = Functions.ServerPath.GetServersServerFiles(serverData.ServerID, StartPath);
            if (!File.Exists(srcdsPath))
            {
                Error = $"{StartPath} not found ({srcdsPath})";
                return null;
            }

            string configPath = Functions.ServerPath.GetServersServerFiles(serverData.ServerID, Game, "cfg/server.cfg");
            if (!File.Exists(configPath))
            {
                Notice = $"server.cfg not found ({configPath})";
            }

            StringBuilder sb = new StringBuilder();
            sb.Append($"-console");
            sb.Append(string.IsNullOrWhiteSpace(Game) ? string.Empty : $" -game {Game}");
            sb.Append(string.IsNullOrWhiteSpace(serverData.ServerIP) ? string.Empty : $" -ip {serverData.ServerIP}");
            sb.Append(string.IsNullOrWhiteSpace(serverData.ServerPort) ? string.Empty : $" -port {serverData.ServerPort}");
            sb.Append(string.IsNullOrWhiteSpace(serverData.ServerMaxPlayer) ? string.Empty : $" -maxplayers{(AppId == "740" ? "_override" : "")} {serverData.ServerMaxPlayer}");
            sb.Append(string.IsNullOrWhiteSpace(serverData.ServerGSLT) ? string.Empty : $" +sv_setsteamaccount {serverData.ServerGSLT}");
            sb.Append(string.IsNullOrWhiteSpace(serverData.ServerParam) ? string.Empty : $" {serverData.ServerParam}");
            sb.Append(string.IsNullOrWhiteSpace(serverData.ServerMap) ? string.Empty : $" +map {serverData.ServerMap}");
            string param = sb.ToString();

            Process p;
            if (!AllowsEmbedConsole)
            {
                p = new Process
                {
                    StartInfo =
                    {
                        FileName = srcdsPath,
                        Arguments = param,
                        WindowStyle = ProcessWindowStyle.Minimized,
                        UseShellExecute = false,
                    },
                    EnableRaisingEvents = true
                };
                p.Start();
            }
            else
            {
                p = new Process
                {
                    StartInfo =
                    {
                        FileName = srcdsPath,
                        Arguments = param,
                        WindowStyle = ProcessWindowStyle.Minimized,
                        UseShellExecute = false,
                        StandardOutputEncoding = Encoding.UTF8,
                        StandardErrorEncoding = Encoding.UTF8,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    },
                    EnableRaisingEvents = true
                };
                var serverConsole = new Functions.ServerConsole(serverData.ServerID);
                p.OutputDataReceived += serverConsole.AddOutput;
                p.ErrorDataReceived += serverConsole.AddOutput;
                p.Start();
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
            }

            return p;
        }

        public async Task Stop(Process p)
        {
            await Task.Run(() =>
            {
                Functions.ServerConsole.SendMessageToMainWindow(p.MainWindowHandle, "quit");
            });
        }

        public async void CreateServerCFG()
        {
            //Download server.cfg
            string configPath = Functions.ServerPath.GetServersServerFiles(serverData.ServerID, Game, "cfg/server.cfg");
            if (await Functions.Github.DownloadGameServerConfig(configPath, serverData.ServerGame))
            {
                string configText = File.ReadAllText(configPath);
                configText = configText.Replace("{{hostname}}", serverData.ServerName);
                configText = configText.Replace("{{rcon_password}}", serverData.GetRCONPassword());
                File.WriteAllText(configPath, configText);
            }

            //Edit WindowsGSM.cfg
            string configFile = Functions.ServerPath.GetServersConfigs(serverData.ServerID, "WindowsGSM.cfg");
            if (File.Exists(configFile))
            {
                string configText = File.ReadAllText(configFile);
                configText = configText.Replace("{{clientport}}", (int.Parse(serverData.ServerPort) - 10).ToString());

                string param = serverData.ServerParam ?? string.Empty;
                bool enableSteamNetworking = param.IndexOf("enable_steam_networking", StringComparison.OrdinalIgnoreCase) >= 0;
                bool customWeapons = param.IndexOf("customweapons", StringComparison.OrdinalIgnoreCase) >= 0;
                bool customRules = param.IndexOf("customrules", StringComparison.OrdinalIgnoreCase) >= 0;

                string steamNetLine = $"sv_use_steam_networking {(enableSteamNetworking ? 1 : 0)}";

                string tagsExtra = string.Empty;
                if (customWeapons) tagsExtra = string.IsNullOrWhiteSpace(tagsExtra) ? "type_customweapons" : tagsExtra + " type_customweapons";
                if (customRules) tagsExtra = string.IsNullOrWhiteSpace(tagsExtra) ? "type_customrules" : tagsExtra + " type_customrules";

                if (configText.Contains("{{enable_steam_networking}}"))
                {
                    configText = configText.Replace("{{enable_steam_networking}}", steamNetLine);
                }
                else
                {
                    configText += Environment.NewLine + steamNetLine;
                }

                if (configText.Contains("{{sv_tags_extra}}"))
                {
                    configText = configText.Replace("{{sv_tags_extra}}", tagsExtra);
                }
                else if (!string.IsNullOrWhiteSpace(tagsExtra))
                {
                    configText += Environment.NewLine + $"// sv_tags additions: {tagsExtra}";
                }

                File.WriteAllText(configFile, configText);
            }
        }

        public new bool IsInstallValid()
        {
            string checkPath = StartPath ?? "srcds_win64.exe";
            string installPath = Functions.ServerPath.GetServersServerFiles(serverData.ServerID, checkPath);
            Error = $"Fail to find {installPath}";
            return File.Exists(installPath);
        }
    }
}
