using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeBodyRadius : Node
    {
        protected override void OnCreate()
        {
            Out<double>("Radius");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Radius", VesselController.Vessel.mainBody.Radius);
        }
    }
}
