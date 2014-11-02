using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
using KSPComputer.Helpers;
namespace DefaultNodes
{
    [Serializable]
    public class NodeSurfaceVelocity : Node
    {
        protected override void OnCreate()
        {
            Out<double>("Lat");
            Out<double>("Long");
            Out<double>("Vert");
        }
        protected override void OnUpdateOutputData()
        {
            Vector3 dir = VesselController.WorldToReference(VesselController.Velocity, VesselController.FrameOfReference.Navball);
            Out("Lat", (double)dir.z);
            Out("Long", (double)dir.x);
            Out("Vert", (double)dir.y);
        }
    }
}
