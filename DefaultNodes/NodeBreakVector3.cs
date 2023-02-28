using System;
using System.Collections.Generic;
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
            In<SVector3d>("Vector3");
            Out<double>("X E/W Long");
            Out<double>("Y U/D Alt");
            Out<double>("Z N/S Lat");
        }
        protected override void OnUpdateOutputData()
        {
            var v = In("Vector3").AsVector3();
            Out("X E/W Long", v.x);
            Out("Y U/D Alt", v.y);
            Out("Z N/S Lat", v.z);
        }
    }
}
