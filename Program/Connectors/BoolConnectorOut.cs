using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.Connectors
{
    [Serializable]
    public class BoolConnectorOut : ConnectorOut
    {
        public BoolConnectorOut()
            : base(typeof(bool), true)
        {
        }
    }
}
