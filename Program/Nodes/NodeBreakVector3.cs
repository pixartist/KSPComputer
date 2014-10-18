using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeBreakVector3 : Node
    {
        public new static string Name = "Break Vector3";
        public new static string Description = "Returns the float components of a Vector3";
        public new static SVector3 Color = new SVector3(0.2f, 1f, 1f);
        public new static SVector2 Size = new SVector2(190, 150);
        protected override void OnCreate()
        {
            In<SVector3>("Vector3");
            Out<float>("X");
            Out<float>("Y");
            Out<float>("Z");
        }
        protected override void OnUpdateOutputData()
        {
            var v = In("Vector3").AsVector3();
            Out("X", v.x);
            Out("Y", v.y);
            Out("Z", v.z);
        }
    }
}
