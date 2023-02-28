using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;
using KSPComputer.Nodes;
using KSPComputer.Types;
namespace DefaultNodes
{
    [Serializable]
    public class NodeNormalizeVector3 : Node
    {
        protected override void OnCreate()
        {
            In<SVector3d>("Vector3");
            Out<SVector3d>("Vector3");
        }
        protected override void OnUpdateOutputData()
        {
            var v = In("Vector3").AsVector3();
            double m = Math.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
            if (m == 0)
                m = 1;
            Out("Vector3", new SVector3d(v.x/m, v.y/m, v.z/m));
        }
    }
}
