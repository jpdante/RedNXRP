using System;
using System.IO;
using GTANetworkAPI;
using Newtonsoft.Json;
using RedNXRP.Core;
// ReSharper disable All

namespace RedNXRP {
    public class Init : Script {
        public static string VERSION = "1.0.2";

        private RedNX _redNX;

        public Init() {
        }

        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart() {
            NAPI.Util.ConsoleOutput("[RedNX] Initializing script...");
            NAPI.Server.SetAutoRespawnAfterDeath(false);
            NAPI.Server.SetAutoSpawnOnConnect(false);
            var configPath = Path.Combine(Environment.CurrentDirectory, "rednx.json");
            if (!File.Exists(configPath)) {
                File.WriteAllText(configPath, JsonConvert.SerializeObject(new Config(), Formatting.Indented));
            }
            var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configPath));
            if (!config.ConfigVersion.Equals(VERSION)) {
                NAPI.Util.ConsoleOutput("[RedNX] [ERROR] The current configuration is out of date and may be incompatible!");
                File.Move(configPath, Path.Combine(Environment.CurrentDirectory, $"rednx-{config.ConfigVersion}.json"));
                File.WriteAllText(configPath, JsonConvert.SerializeObject(new Config(config), Formatting.Indented));
                config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configPath));
            }
            _redNX = new RedNX(config);
            _redNX.Start();
            NAPI.Util.ConsoleOutput("[RedNX] Script initialized!");
        }

        [ServerEvent(Event.ResourceStop)]
        public void OnResourceStop() {
            NAPI.Util.ConsoleOutput("[RedNX] Terminating script...");
            _redNX.Stop();
            NAPI.Util.ConsoleOutput("[RedNX] Script terminated!");
        }
    }
}
