using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeApoapsisAltitude : Node
    {
        protected override void OnCreate()
        {
            Out<double>("Apoapsis");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Apoapsis", Program.Vessel.orbit.ApA);
        }
    }
}
