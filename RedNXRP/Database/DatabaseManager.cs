using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using RedNXRP.Scripts.NXFuel;

namespace RedNXRP.Database {
    public class DatabaseManager {

        private readonly MySqlConnection _connection;

        public DatabaseManager(string type, string connectionString) {
            if (type.Equals("mysql", StringComparison.InvariantCultureIgnoreCase)) {
                _connection = new MySqlConnection(connectionString);
            }
        }

        public int CreateGasStation(GasStation station, out int id) {
            if(_connection.State != ConnectionState.Open) _connection.Open();
            var command = new MySqlCommand("INSERT INTO `gas_stations` (`ID`, `Location`, `ProviderLocations`) VALUES (@id, @location, @providers);", _connection);
            command.Parameters.AddWithValue("@id", station.ID);
            command.Parameters.AddWithValue("@location", JsonConvert.SerializeObject(station.Location));
            command.Parameters.AddWithValue("@providers", JsonConvert.SerializeObject(station.ProviderLocations));
            var result = command.ExecuteNonQuery();
            id = (int)command.LastInsertedId;
            return result;
        }

        public int UpdateGasStation(GasStation station) {
            if(_connection.State != ConnectionState.Open) _connection.Open();
            var command = new MySqlCommand("UPDATE `gas_stations` SET `Location` = @location, `ProviderLocations` = @providers  WHERE `gas_stations`.`ID` = @id;", _connection);
            command.Parameters.AddWithValue("@id", station.ID);
            command.Parameters.AddWithValue("@location", JsonConvert.SerializeObject(station.Location));
            command.Parameters.AddWithValue("@providers", JsonConvert.SerializeObject(station.ProviderLocations));
            return command.ExecuteNonQuery();
        }

        public List<GasStation> GetGasStations() {
            if(_connection.State != ConnectionState.Open) _connection.Open();
            var stations = new List<GasStation>();
            var command = new MySqlCommand("SELECT * FROM `gas_stations`;", _connection);
            using (var reader = command.ExecuteReader()) {
                if (!reader.HasRows) return stations;
                while (reader.Read())  {
                    var gasStation = new GasStation {
                        ID = reader.GetInt32("ID"),
                        Location = JsonConvert.DeserializeObject<Vector3>(reader.GetString("Location")),
                        ProviderLocations = JsonConvert
                            .DeserializeObject<Vector3[]>(reader.GetString("ProviderLocations")).ToList()
                    };
                    stations.Add(gasStation);
                }
            }
            return stations;
        }

    }
}
