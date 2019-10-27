using System;
using System.Collections.Generic;
using System.Text;

namespace RedNXRP.Database.Databases {
    public abstract class BaseDataBase : IDisposable {

        public abstract void Connect();
        public abstract void Dispose();

    }
}
