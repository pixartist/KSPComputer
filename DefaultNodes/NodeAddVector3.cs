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
    public class NodeAddVector3 : Node
    {
        protected override void OnCreate()
        {
            In<SVector3d>("A");
            In<SVector3d>("B");
            Out<SVector3d>("Result");
        }
        protected override void OnUpdateOutputData()
        {
            var v1 = In("A").AsVector3();
            var v2 = In("B").AsVector3();
            Out("Result", new SVector3d(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z));
        }
    }
}
