using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Types;
using KSPComputer.Nodes;
namespace DefaultNodes
{
    [Serializable]
    public class NodeBreakVector3 : Node
    {
        protected override void OnCreate()
        {
            In<SVector3>("Vector3");
            Out<double>("X");
            Out<double>("Y");
            Out<double>("Z");
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
