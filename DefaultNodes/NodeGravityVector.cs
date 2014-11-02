using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Types;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeGravityVector : Node
    {
        protected override void OnCreate()
        {
            Out<SVector3>("Gravity");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Gravity", new SVector3(VesselController.GravityVector));
        }
    }
}
