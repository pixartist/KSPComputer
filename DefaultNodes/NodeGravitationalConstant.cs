using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeGravitationalConstant : Node
    {
        protected override void OnCreate()
        {
            Out<double>("G");
        }
        protected override void OnUpdateOutputData()
        {
            Out("G", VesselController.G);
        }
    }
}
