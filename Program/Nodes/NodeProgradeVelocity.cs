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
        public new static string Name = "Prograde Velocity";
        public new static string Description = "Returns the velocity along the forward axis";
        public new static SVector3 Color = new SVector3(0.5f, 1f, 0.5f);
        public new static SVector2 Size = new SVector2(190, 50);
        protected override void OnCreate()
        {
            AddConnectorOut("Velocity", new FloatConnectorOut());
        }
        protected override void OnUpdateOutputData()
        {
            var c = GetConnectorOut("Velocity");
            if (c != null)
            {
                c.SendData(Vector3.Dot(Program.Vessel.ReferenceTransform.up, Program.Vessel.obt_velocity));
            }
        }
    }
}
