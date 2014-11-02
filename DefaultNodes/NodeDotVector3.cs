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
    public class NodeDotVector3 : Node
    {
        protected override void OnCreate()
        {
            In<SVector3>("A");
            In<SVector3>("B");
            Out<double>("Dot");
        }
        protected override void OnUpdateOutputData()
        {
            var a = In("A").AsVector3().GetVec3();
            var b = In("B").AsVector3().GetVec3();
            Out("Dot", (double)Vector3.Dot(a,b));
        }
    }
}
