using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeProgradeVelocity : Node
    {
        protected override void OnCreate()
        {
            Out<double>("Velocity");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Velocity", (double)Vector3.Dot(Program.Vessel.ReferenceTransform.up, Program.Vessel.obt_velocity));
        }
    }
}
