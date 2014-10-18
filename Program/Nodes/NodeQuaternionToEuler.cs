using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeQuaternionToEuler : Node
    {
        public new static string Name = "Quaternion to euler";
        public new static string Description = "Returns the euler-angles for a quaternion";
        public new static SVector3 Color = new SVector3(0.2f, 1f, 1f);
        public new static SVector2 Size = new SVector2(190, 200);
        protected override void OnCreate()
        {
            In<SQuaternion>("Quaternion");
            Out<float>("X");
            Out<float>("Y");
            Out<float>("Z");
            
        }
        protected override void OnUpdateOutputData()
        {
            var v = In("Quaternion").AsQuaternion().GetQuaternion().eulerAngles;
            Out("X", v.x);
            Out("Y", v.y);
            Out("Z", v.z);
        }
    }
}
