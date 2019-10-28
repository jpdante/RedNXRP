using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace RedNXRP.Scripts.NXFuel {
    public class GasStation {
        public int ID;
        public Vector3 Location { get; set; }
        public List<Vector3> ProviderLocations { get; set; }

        public GasStation() {
            ProviderLocations = new List<Vector3>();
        }
    }
}
