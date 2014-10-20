using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodePeriapsis : Node
    {
        protected override void OnCreate()
        {
            Out<double>("Periapsis");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Periapsis", Program.Vessel.orbit.PeA);
        }
    }
}
