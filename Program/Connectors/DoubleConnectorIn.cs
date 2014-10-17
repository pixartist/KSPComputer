using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.Connectors
{
    [Serializable]
    public class DoubleConnectorIn : ConnectorIn
    {
        public DoubleConnectorIn()
            : base(typeof(double), false)
        {
        }
    }
}