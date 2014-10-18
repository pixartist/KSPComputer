using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeMakeVector3 : Node
    {
        public new static string Name = "Make Vector3";
        public new static string Description = "Creates a Vector3 from three floats";
        public new static SVector3 Color = new SVector3(0.2f, 1f, 1f);
        public new static SVector2 Size = new SVector2(190, 200);
        protected override void OnCreate()
        {
            Out<SVector3>("Vector3");
            In<float>("X");
            In<float>("Y");
            In<float>("Z");
        }
        protected override void OnUpdateOutputData()
        {
            var x = In("X").AsFloat();
            var y = In("Y").AsFloat();
            var z = In("Z").AsFloat();
            Out("Vector3", new SVector3(x, y, z));
        }
    }
}
