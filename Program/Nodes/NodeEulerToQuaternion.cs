using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeEulerToQuaternion : Node
    {
        public new static string Name = "Euler to quaternion";
        public new static string Description = "Creates a quaternion from euler-angles";
        public new static SVector3 Color = new SVector3(0.2f, 1f, 1f);
        public new static SVector2 Size = new SVector2(190, 200);
        protected override void OnCreate()
        {
            Out<SQuaternion>("Quaternion");
            In<float>("X");
            In<float>("Y");
            In<float>("Z");
        }
        protected override void OnUpdateOutputData()
        {
            var x = In("X").AsFloat();
            var y = In("Y").AsFloat();
            var z = In("Z").AsFloat();
            Out("Quaternion", new SQuaternion(Quaternion.Euler(x, y, z)));
        }
    }
}
