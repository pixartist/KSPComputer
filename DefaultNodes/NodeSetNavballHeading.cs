using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPComputer;
using KSPComputer.Helpers;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeSetNavballHeading : DefaultExecutableNode
    {
        protected override void OnCreate()
        {
            In<double>("N/S");
            In<double>("E/W");
            In<double>("U/D");
            In<double>("Roll");
            In<bool>("Keep Roll");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            //get target vector(navball)
            Vector3 v = new Vector3(In("E/W").AsFloat(), In("U/D").AsFloat(), In("N/S").AsFloat()).normalized;
            //create rotation
            Quaternion rot = Quaternion.LookRotation(v, Vector3.up) * Quaternion.Euler(90, 0, 0);
            Quaternion roll = Quaternion.identity;
            //keep absolute roll ?
            if (In("Keep Roll").AsBool())
            {

                //get navball horizontal angles
                float angleC = Mathf.Atan2(VesselController.NavballHeading.z, VesselController.NavballHeading.x);

                float angleT = Mathf.Atan2(v.z, v.x);

                //delta of angles
                float d = Mathf.DeltaAngle(angleC, angleT) * Mathf.Rad2Deg;
                //add delta to current angle
                roll = Quaternion.AngleAxis((float)VesselController.Roll + d, v);
            }
            else
            {
                roll = Quaternion.AngleAxis(In("Roll").AsFloat(), v);
            }
            rot = roll * rot;
            //apply sas target
            SASController.SASTarget = VesselController.ReferenceToWorld(rot, VesselController.FrameOfReference.Navball);
            ExecuteNext();
        }
    }
}