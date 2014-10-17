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
            AddConnectorOut("Quaternion", new QuaternionConnectorOut());
            AddConnectorIn("X", new FloatConnectorIn());
            AddConnectorIn("Y", new FloatConnectorIn());
            AddConnectorIn("Z", new FloatConnectorIn());
        }
        protected override void OnUpdateOutputData()
        {
            var v = GetConnectorOut("Quaternion");
            if (v != null)
            {
                var x = GetConnectorIn("X").GetBufferAsFloat();
                var y = GetConnectorIn("Y").GetBufferAsFloat();
                var z = GetConnectorIn("Z").GetBufferAsFloat();
                v.SendData(new SQuaternion(Quaternion.Euler(x,y,z)));
            }
        }
    }
}
