using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodePeriapsisAltitude : Node
    {
        protected override void OnCreate()
        {
            Out<double>("Periapsis");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Periapsis", Vessel.orbit.PeA);
        }
    }
}
