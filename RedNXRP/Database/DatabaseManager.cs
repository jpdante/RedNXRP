using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using RedNXRP.Database.Databases;

namespace RedNXRP.Database {
    public class DatabaseManager {

        private BaseDataBase _database;

        public DatabaseManager(string type, string connectionString) {
            if (type.Equals("mysql", StringComparison.InvariantCultureIgnoreCase)) {
                _database = new MySQL(connectionString);
            } else if (type.Equals("mysql", StringComparison.InvariantCultureIgnoreCase)) {

            }
        }

    }
}
