using System;
using System.Collections.Generic;

using System.Text;
using KSPComputer.Helpers;
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
            Out<SVector3d>("Gravity");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Gravity", new SVector3d(VesselController.WorldToReference(VesselController.GravityVector, VesselController.FrameOfReference.Navball)));
        }
    }
}
