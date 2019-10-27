using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using RedNXRP.Database;

namespace RedNXRP.Core {
    public class RedNX {
        public static Config Config { get; private set; }
        public DatabaseManager DatabaseManager { get; private set; }

        public RedNX(Config config) {
            Config = config;
            DatabaseManager = new DatabaseManager(config.Database.Type, config.Database.ConnectionString);
        }

        public void Start() {
        }

        public void Stop() {

        }
    }
}
