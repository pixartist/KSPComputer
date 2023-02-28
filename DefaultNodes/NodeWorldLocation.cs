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
    public class NodeWorldLocation : Node
    {
        protected override void OnCreate()
        {
            Out<SVector3d>("Location");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Location", new SVector3d(VesselController.WorldToReference(VesselController.CenterOfMass, VesselController.FrameOfReference.Navball)));
        }
    }
}
