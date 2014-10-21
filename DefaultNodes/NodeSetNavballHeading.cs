using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPComputer.Helpers;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeSetNavballHeading : ExecutableNode
    {
        protected override void OnCreate()
        {
            In<double>("N/S");
            In<double>("E/W");
            In<double>("U/D");
            In<double>("Roll");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            Vector3 v = new Vector3(In("E/W").AsFloat(), In("U/D").AsFloat(), In("N/S").AsFloat()).normalized;
            Quaternion rot = Quaternion.LookRotation(v, Vector3.up) * Quaternion.Euler(90, 0, 0);
            rot = Quaternion.AngleAxis(In("Roll").AsFloat(), v) * rot;
            

            
            //WHY Quaternion.Euler(90, 0, 0) you ask ? BECAUSE KSP USES UP AS FORWARD, those IDIOTS
            Program.SASController.SASTarget = Program.VesselInfo.ReferenceToWorld(rot, VesselInformation.FrameOfReference.Navball);
            ExecuteNext();
        }
    }
}