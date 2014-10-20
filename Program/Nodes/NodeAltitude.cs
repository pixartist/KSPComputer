using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeAltitude : Node
    {
        protected override void OnCreate()
        {
            Out<double>("Altitude");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Altitude", Program.Vessel.altitude);
        }
    }
}
