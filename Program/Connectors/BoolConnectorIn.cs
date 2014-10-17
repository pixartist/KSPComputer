using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.Connectors
{
    [Serializable]
    public class BoolConnectorIn : ConnectorIn
    {
        public BoolConnectorIn()
            : base(typeof(bool), false)
        {
        }
    }
}