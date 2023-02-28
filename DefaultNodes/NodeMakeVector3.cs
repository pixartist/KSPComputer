using System;
using System.Collections.Generic;

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
            Out<SVector3d>("Vector3");
            In<double>("X E/W Long");
            In<double>("Y U/D Alt");
            In<double>("Z N/S Lat");
        }
        protected override void OnUpdateOutputData()
        {
            var x = In("X E/W Long").AsDouble();
            var y = In("Y U/D Alt").AsDouble();
            var z = In("Z N/S Lat").AsDouble();
            Out("Vector3", new SVector3d(x, y, z));
        }
    }
}
