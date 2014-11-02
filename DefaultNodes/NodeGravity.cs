using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeGravity : Node
    {
        protected override void OnCreate()
        {
            Out<double>("Gravity");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Gravity", VesselController.CurrentGravity);
        }
    }
}
