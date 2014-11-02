using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPComputer;
using KSPComputer.Types;
using KSPComputer.Helpers;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeSetHeading : DefaultExecutableNode
    {
        protected override void OnCreate()
        {
            In<SVector3>("Forward");
            In<SVector3>("Up");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            SASController.SASTarget = Quaternion.LookRotation(In("Forward").AsVector3().GetVec3(),
                In("Up").AsVector3().GetVec3());
            ExecuteNext();
        }
    }
}