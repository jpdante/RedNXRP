using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using RedNXRP.Core;

namespace RedNXRP.Scripts.NXFuel {
    public class NXFuel : Script {

        public Dictionary<int, GasStation> GasStations;
        public List<Blip> Blips;
        public List<Marker> Markers;

        public NXFuel() {
            GasStations = new Dictionary<int, GasStation>();
            Blips = new List<Blip>();
            Markers = new List<Marker>();
        }

        public void LoadGasStations() {
            foreach (var station in RedNX.Context.DatabaseManager.GetGasStations()) {
                GasStations.Add(station.ID, station);
            }
        }

        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart() {
            /*NAPI.Blip.CreateBlip(361, new Vector3(620.5615, 268.8869, 103.0895), 1f, 1, "Posto Shell", 255, 0F, true);
            NAPI.Blip.CreateBlip(361, new Vector3(2581.007, 361.7741, 108.4688), 1f, 1, "Posto Shell", 255, 0F, true);
            NAPI.Blip.CreateBlip(361, new Vector3(1702.204, 6416.899, 32.76404), 1f, 1, "Posto Shell", 255, 0F, true);
            NAPI.Marker.CreateMarker(1, new Vector3(618.7195, 264.2259, 101), new Vector3(), new Vector3(), 3, new Color(255, 255, 255));*/
            NAPI.Util.ConsoleOutput("[NXFuel] Initializing script...");
            LoadGasStations();
            RefreshBlipsAndMarkers();
            NAPI.Util.ConsoleOutput("[NXFuel] Script initialized!");
        }

        [Command("create-station")]
        public void CreateStation(Client client) {
            var station = new GasStation {
                Location = client.Position.Copy(),
            };
            GasStations.Add(station.ID, station);
            RedNX.Context.DatabaseManager.CreateGasStation(station, out station.ID);
            NAPI.Notification.SendNotificationToPlayer(client, $"Gas Station {station.ID} created!");
            RefreshBlipsAndMarkers();
        }

        [Command("create-station-provider")]
        public void CreateStationProvider(Client client, int stationId) {
            if (GasStations.TryGetValue(stationId, out var station)) {
                station.ProviderLocations.Add(client.Position.Copy());
                RedNX.Context.DatabaseManager.UpdateGasStation(station);
                NAPI.Notification.SendNotificationToPlayer(client, $"Gas Station Provider {station.ProviderLocations.Count} created!");
                RefreshBlipsAndMarkers();
            } else {
                NAPI.Chat.SendChatMessageToPlayer(client, $"~r~Station '{stationId}' not found!");
            }
        }

        public void RefreshBlipsAndMarkers() {
            foreach (var blip in Blips) {
                blip.Delete();
            }
            foreach (var marker in Markers) {
                marker.Delete();
            }
            Blips.Clear();
            Markers.Clear();
            foreach (var station in GasStations.Values) {
                Blips.Add(NAPI.Blip.CreateBlip(361, station.Location, 1f, 1, "Posto Shell", 255, 0F, true));
            }
            foreach (var provider in GasStations.Values.SelectMany(station => station.ProviderLocations)) {
                Markers.Add(NAPI.Marker.CreateMarker(1, provider.Subtract(new Vector3(0,0,2)), new Vector3(), new Vector3(), 3f, new Color(255, 0, 0)));
            }
        }
    }
}
