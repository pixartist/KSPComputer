using System;
using System.Collections.Generic;
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
            Out("Apoapsis", Vessel.orbit.ApA);
        }
    }
}
