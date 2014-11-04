using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPComputer.Helpers;
using KSPComputer.Types;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeVelocity : Node
    {
        protected override void OnCreate()
        {
            Out<SVector3d>("Velocity");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Velocity", new SVector3d(VesselController.WorldToReference(VesselController.Velocity, VesselController.FrameOfReference.Navball)));
        }
    }
}
