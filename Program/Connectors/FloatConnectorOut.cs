using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.Connectors
{
    [Serializable]
    public class FloatConnectorOut : ConnectorOut
    {
        public FloatConnectorOut()
            : base(typeof(float), true)
        {
        }
    }
}