using System;
using System.Collections.Generic;

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
    public class NodeSetHeadingRoll : DefaultExecutableNode
    {
        protected override void OnCreate()
        {
            In<SVector3d>("Forward");
            In<double>("Roll");
            
        }
        protected override void OnExecute(ConnectorIn input)
        {
            //get target vector(navball)
            Vector3 v = In("Forward").AsVector3().GetVec3().normalized;
            //create rotation
            Quaternion rot = Quaternion.LookRotation(v, Vector3.up) * Quaternion.Euler(90, 0, 0);
            Quaternion roll = Quaternion.identity;
            //keep absolute roll ?

            roll = Quaternion.AngleAxis(In("Roll").AsFloat(), v);

            rot = roll * rot;
            //apply sas target
            SASController.SetSASTarget(rot);
            ExecuteNext();
        }
    }
}