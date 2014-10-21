using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Types;
namespace DefaultNodes
{
    [Serializable]
    public class NodeMakeVector3 : Node
    {
        protected override void OnCreate()
        {
            Out<SVector3>("Vector3");
            In<double>("X");
            In<double>("Y");
            In<double>("Z");
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
