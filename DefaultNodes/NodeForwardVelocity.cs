using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeForwardVelocity : Node
    {
        protected override void OnCreate()
        {
            Out<double>("Velocity");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Velocity", (double)Vector3.Dot(Vessel.ReferenceTransform.up, VesselController.Velocity));
        }
    }
}
