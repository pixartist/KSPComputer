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
            In<SVector3d>("Forward");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            //get target vector(navball)
            Vector3 v = In("Forward").AsVector3().GetVec3().normalized;
            //create rotation
            Quaternion rot = Quaternion.LookRotation(v, Vector3.up) * Quaternion.Euler(90, 0, 0);
            Quaternion roll = Quaternion.identity;
            //keep absolute roll ?
            //get navball horizontal angles
            float angleC = Mathf.Atan2(VesselController.NavballHeading.z, VesselController.NavballHeading.x);

            float angleT = Mathf.Atan2(v.z, v.x);

            //delta of angles
            float d = Mathf.DeltaAngle(angleC, angleT) * Mathf.Rad2Deg;
            //add delta to current angle
            roll = Quaternion.AngleAxis((float)VesselController.Roll + d, v);

            rot = roll * rot;
            //apply sas target
            SASController.SASTarget = VesselController.ReferenceToWorld(rot, VesselController.FrameOfReference.Navball);
            ExecuteNext();
        }
    }
}