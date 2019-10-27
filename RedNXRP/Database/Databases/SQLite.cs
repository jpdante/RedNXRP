using System;
using System.Collections.Generic;
using System.Text;

namespace RedNXRP.Database.Databases {
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once InconsistentNaming
    public class SQLite : BaseDataBase {

        private string _connectionString;

        public SQLite(string connectionString) { _connectionString = connectionString; }

        public override void Connect() {

        }

        public override void Dispose() {

        }
    }
}
