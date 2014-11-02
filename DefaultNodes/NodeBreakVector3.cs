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
            Out<double>("X E/W");
            Out<double>("Y U/D");
            Out<double>("Z N/S");
        }
        protected override void OnUpdateOutputData()
        {
            var v = In("Vector3").AsVector3();
            Out("X E/W", v.x);
            Out("Y U/D", v.y);
            Out("Z N/S", v.z);
        }
    }
}
