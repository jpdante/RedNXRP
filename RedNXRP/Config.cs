using System;
using System.Collections.Generic;
using System.Text;

namespace RedNXRP {
    [Serializable]
    public class Config {
        public string ConfigVersion { get; set; }
        public DatabaseConfig Database { get; set; }

        public Config() {
            ConfigVersion = Init.VERSION;
            Database = new DatabaseConfig();
        }

        public Config(Config config) {
            ConfigVersion = Init.VERSION;
            Database = DatabaseConfig.CheckFix(config.Database);
        }
    }

    [Serializable]
    public class DatabaseConfig {
        public string Type { get; set; }
        public string ConnectionString { get; set; }

        public DatabaseConfig() {
            Type = "mysql";
            ConnectionString = "Server=127.0.0.1;Database=test;Uid=root;Pwd=root;";
        }

        public static DatabaseConfig CheckFix(DatabaseConfig config) {
            return config ?? new DatabaseConfig();
        }
    }
}
