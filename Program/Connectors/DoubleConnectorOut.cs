using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.Connectors
{
    [Serializable]
    public class DoubleConnectorOut : ConnectorOut
    {
        public DoubleConnectorOut()
            : base(typeof(double), true)
        {
        }
    }
}