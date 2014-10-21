using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
using KSPComputer.Types;
namespace DefaultNodes
{
    [Serializable]
    public class NodeQuaternionToEuler : Node
    {
        protected override void OnCreate()
        {
            In<SQuaternion>("Quaternion");
            Out<double>("X");
            Out<double>("Y");
            Out<double>("Z");
            
        }
        protected override void OnUpdateOutputData()
        {
            var v = In("Quaternion").AsQuaternion().GetQuaternion().eulerAngles;
            Out("X", (double)v.x);
            Out("Y", (double)v.y);
            Out("Z", (double)v.z);
        }
    }
}
