using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using GTANetworkAPI;

namespace RedNXRP.Scripts.HelloWorldTest {
    public class HelloWorldTest : Script {

        public Dictionary<Client, UserData> users;
        private Timer timer;
        private Timer timer2;

        public HelloWorldTest() {
            NAPI.Util.ConsoleOutput("[HelloWorldTest] Constructing script...");
            users = new Dictionary<Client, UserData>();
            timer = new Timer();
            timer.Interval = 50;
            timer.Elapsed += TimerOnElapsed;
            timer.Start();

            timer2 = new Timer();
            timer2.Interval = 200;
            timer2.Elapsed += TimerOnElapsed2;
            timer2.Start();
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e) {
            foreach (var client in users.Keys) {
                NAPI.Task.Run(() => {
                    users[client].blip?.Delete();
                    users[client].blip = NAPI.Blip.CreateBlip(users[client].type, client.Position, 1, 1, client.Name);
                });
            }
        }

        private void TimerOnElapsed2(object sender, ElapsedEventArgs e) {
            foreach (var client in users.Keys) {
                if (!users[client].Animated) continue;
                users[client].type++;
                if (users[client].type >= 100) users[client].type = 0;
            }
        }

        [ServerEvent(Event.PlayerDeath)]
        public void OnPlayerDeath(Client client, Client killer, uint reason) {
            NAPI.Notification.SendNotificationToAll($"{killer.Name} matou {client.Name}");
            if (users.ContainsKey(client)) {
                users[client].respawn = client.Position;
                users[client].KDA--;
                NAPI.Notification.SendNotificationToPlayer(client, $"KDA: {users[client].KDA}");
            }
            if (!users.ContainsKey(killer)) return;
            users[killer].KDA++;
            NAPI.Notification.SendNotificationToPlayer(killer, $"KDA: {users[killer].KDA}");
        }

        [ServerEvent(Event.PlayerSpawn)]
        public void OnPlayerSpawn(Client client) {
            if (!users.ContainsKey(client)) {
                users.Add(client, new UserData());
            }
            client.Position = new Vector3(618.7195, 264.2259, 103.0895);
        }

        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart() {
            NAPI.Util.ConsoleOutput("[HelloWorldTest] Initializing script...");
            NAPI.Util.ConsoleOutput("[HelloWorldTest] Script initialized!");
        }

        [Command("car")]
        public void SpawnVehicle(Client client, VehicleHash carType) {
            var vehicle = NAPI.Vehicle.CreateVehicle(carType, client.Position, new Vector3(), 1, 1, "123");
            NAPI.Player.SetPlayerIntoVehicle(client, vehicle, -1);
            NAPI.Notification.SendNotificationToPlayer(client, $"{carType} Spawned!");
        }

        [Command("cords")]
        public void ShowCords(Client client) {
            NAPI.Chat.SendChatMessageToAll($"{client.Position.X} {client.Position.Y} {client.Position.Z}");
        }

        [Command("tp")]
        public void TP(Client client, float x, float y, float z) {
            NAPI.Notification.SendNotificationToPlayer(client, $"Teleportanto para XYZ");
            client.Position = new Vector3(x, y, z);
        }

        [Command("tpu")]
        public void TPU(Client client, string name) {
            foreach (var user in users.Keys) {
                if (user.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)) {
                    NAPI.Notification.SendNotificationToPlayer(client, $"Teleportanto para " + name);
                    client.Position = user.Position;
                    return;
                }
            }
        }

        [Command("spawn")]
        public void TeleportSpawn(Client client) {
            client.Position = new Vector3(-1582, -2615, 14);
        }

        [Command("setblip")]
        public void SetBlip(Client client, int i) {
            if (i <= 1) {
                users[client].Animated = true;
            } else {
                users[client].type = i;
                users[client].Animated = false;
            }
        }

        [Command("setname")]
        public void SetName(Client client, string name) { NAPI.Player.SetPlayerName(client, name); }

        [Command("blip")]
        public void SpawnBlip(Client client, int sprite, float scale, string name) { NAPI.Blip.CreateBlip(sprite, client.Position, scale, 1, name); }

        [Command("explode")]
        public void SpawnExplode(Client client, string explosionName) {
            if (!Enum.TryParse(explosionName, out ExplosionType explosionType)) {
                NAPI.Chat.SendChatMessageToAll("Explosion type not found!");
                return;
            }
            NAPI.Explosion.CreateExplosion(explosionType, client.Position);
        }

        [Command("ped")]
        public void SpawnPed(Client client, PedHash pedType, float heading) {
            NAPI.Ped.CreatePed(pedType, client.Position, heading);
        }

        [Command("gun")]
        public void SpawnGun(Client client, WeaponHash weaponType, int ammo) {
            client.GiveWeapon(weaponType, ammo);
            NAPI.Notification.SendNotificationToPlayer(client, $"{weaponType} gived!");
        }

        [Command("random")]
        public void Random(Client client, PedHash pedType) {
            client.WantedLevel = 5;
            client.SetSkin(pedType);
        }

        [Command("respawn")]
        public void Respawn(Client client) {
            NAPI.Player.SpawnPlayer(client, new Vector3(13, 11, 32));
            NAPI.Entity.SetEntityInvincible(client, false);
        }

        /*[ServerEvent(Event.ChatMessage)]
        public void OnChatMessage(Client client, string message) {
            if (message != null || message.Length <= 0) return;
            var data = message.Split(" ");
            if (data[0].Equals("car", StringComparison.InvariantCultureIgnoreCase)) {
                NAPI.Chat.SendChatMessageToAll("Spawning car...");
                if (data.Length != 2) return;
                if (!Enum.TryParse(data[1], out VehicleHash carType)) return;
                var vehicle = NAPI.Vehicle.CreateVehicle(carType, client.Position, new Vector3(), 1, 1, "123");
                NAPI.Player.SetPlayerIntoVehicle(client, vehicle, -1);
                NAPI.Notification.SendNotificationToPlayer(client, $"{carType} Spawned!");
            }*/
        /*
    if (data[0].Equals("blip", StringComparison.InvariantCultureIgnoreCase)) {
        var blip = NAPI.Blip.CreateBlip(client.Position);
        blip.Name = "CU";
        blip.Scale = 2;
    } else if (data[0].Equals("marker", StringComparison.InvariantCultureIgnoreCase)) {
        var marker = NAPI.Marker.CreateMarker(MarkerType.ReplayIcon, client.Position, new Vector3(),
            new Vector3(), 1, new Color(255, 0, 70));
    } else if (data[0].Equals("ped", StringComparison.InvariantCultureIgnoreCase)) {
        var ped = NAPI.Ped.CreatePed(PedHash.AmmuCountrySMM, client.Position, 1);
    } else if (data[0].Equals("label", StringComparison.InvariantCultureIgnoreCase)) {
        var label = NAPI.TextLabel.CreateTextLabel("MEU PAU NA SUA MAO", client.Position, 20, 20, 1,
            new Color(255, 255, 255));
    } else if (data[0].Equals("car", StringComparison.InvariantCultureIgnoreCase)) {
        NAPI.Chat.SendChatMessageToAll("Spawning car...");
        if (data.Length != 2) return;
        if (!Enum.TryParse(data[1], out VehicleHash carType)) return;
        var vehicle = NAPI.Vehicle.CreateVehicle(carType, client.Position, new Vector3(), 1, 1, "123");
        NAPI.Player.SetPlayerIntoVehicle(client, vehicle, -1);
        NAPI.Notification.SendNotificationToPlayer(client, $"{carType} Spawned!");
    } else if (data[0].Equals("setname", StringComparison.InvariantCultureIgnoreCase)) {
        if (data.Length != 2) return;
        NAPI.Player.SetPlayerName(client, data[1]);
    } else if (data[0].Equals("cords", StringComparison.InvariantCultureIgnoreCase)) {
        NAPI.Notification.SendNotificationToAll($"{client.Position.X} {client.Position.Y} {client.Position.Z}");
        NAPI.Chat.SendChatMessageToPlayer(client, $"{client.Position.X} {client.Position.Y} {client.Position.Z}");
    } else if (data[0].Equals("tp", StringComparison.InvariantCultureIgnoreCase)) {
        if (data.Length == 4) {
            if (float.TryParse(data[1], out var x) && float.TryParse(data[2], out var y) && float.TryParse(data[3], out var z)) {
                NAPI.Notification.SendNotificationToPlayer(client, $"Teleportanto para XYZ");
                client.Position = new Vector3(x, y, z);
            } else {
                NAPI.Notification.SendNotificationToPlayer(client, $"Falha ao teleportar!");
            }

        } else if (data.Length == 2) {
            foreach (var player in clients) {
                if (player?.Name != null && player.Name.Equals(data[1])) {
                    NAPI.Notification.SendNotificationToPlayer(client, $"Teleportanto para {data[1]}");
                    client.Position = player.Position;
                    return;
                }
            }
            NAPI.Notification.SendNotificationToPlayer(client, $"Falha ao teleportar!");
        }
    } else if (data[0].Equals("god1", StringComparison.InvariantCultureIgnoreCase)) {
        NAPI.Notification.SendNotificationToPlayer(client, $"GOD ATIVADO!");
        client.Invincible = true;
    } else if (data[0].Equals("god2", StringComparison.InvariantCultureIgnoreCase)) {
        NAPI.Notification.SendNotificationToPlayer(client, $"GOD DESATIVADO!");
        client.Invincible = false;
    }*/
        //}
    }

    public class UserData {
        public Vector3 respawn;
        public int type = 1;
        public Blip blip;
        public bool Animated = false;
        public int KDA;
    }
}
