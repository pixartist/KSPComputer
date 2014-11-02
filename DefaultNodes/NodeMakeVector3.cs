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
            In<double>("X E/W");
            In<double>("Y U/D");
            In<double>("Z N/S");
        }
        protected override void OnUpdateOutputData()
        {
            var x = In("X E/W").AsFloat();
            var y = In("Y U/D").AsFloat();
            var z = In("Z N/S").AsFloat();
            Out("Vector3", new SVector3(x, y, z));
        }
    }
}
