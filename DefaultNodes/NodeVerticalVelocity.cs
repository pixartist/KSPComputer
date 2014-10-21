using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeVerticalVelocity : Node
    {
        protected override void OnCreate()
        {
            Out<double>("Velocity");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Velocity", Program.Vessel.verticalSpeed);
        }
    }
}
