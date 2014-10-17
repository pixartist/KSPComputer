using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.Connectors
{
    [Serializable]
    public class FloatConnectorIn : ConnectorIn
    {
        public FloatConnectorIn()
            : base(typeof(float), false)
        {
        }
    }
}