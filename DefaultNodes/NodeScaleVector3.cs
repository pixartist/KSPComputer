using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPComputer.Nodes;
using KSPComputer.Types;
namespace DefaultNodes
{
    [Serializable]
    public class NodeScaleVector3 : Node
    {
        protected override void OnCreate()
        {
            In<SVector3d>("Vector");
            In<double>("Scale");
            Out<SVector3d>("Result");
        }
        protected override void OnUpdateOutputData()
        {
            var v = In("Vector").AsVector3();
            var s = In("Scale").AsDouble();
            Out("Result", new SVector3d(v.x * s, v.y * s, v.z * s));
        }
    }
}
