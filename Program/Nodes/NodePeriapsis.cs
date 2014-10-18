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
        public new static string Name = "Periapsis";
        public new static string Description = "Returns the current periapsis";
        public new static SVector3 Color = new SVector3(0.5f, 1f, 0.5f);
        public new static SVector2 Size = new SVector2(190, 50);
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
