using System;
using System.Collections.Generic;
using System.Text;

namespace RedNXRP.Database.Databases {
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once InconsistentNaming
    public class MySQL : BaseDataBase {

        private string _connectionString;

        public MySQL(string connectionString) { _connectionString = connectionString; }

        public override void Connect() {

        }

        public override void Dispose() {

        }
    }
}
