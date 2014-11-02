using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Helpers;
using KSPComputer.Types;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeLocation : Node
    {
        protected override void OnCreate()
        {
            Out<SVector3>("Location");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Location", new SVector3(VesselController.WorldToReference(VesselController.CenterOfMass, VesselController.FrameOfReference.Navball)));
        }
    }
}
