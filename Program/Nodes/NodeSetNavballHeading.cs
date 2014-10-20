using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeSetNavballHeading : ExecutableNode
    {
        public new static string Name = "Set navball heading";
        public new static string Description = "Sets the heading relative to the orbited body";
        public new static SVector3 Color = new SVector3(1, 1, 0.2f);
        public new static SVector2 Size = new SVector2(150,300);
        protected override void OnCreate()
        {
            In<float>("N/S");
            In<float>("E/W");
            In<float>("U/D");
            In<float>("Roll");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            Vector3 v = new Vector3(In("E/W").AsFloat(), In("U/D").AsFloat(), In("N/S").AsFloat()).normalized;
            Quaternion rot = Quaternion.LookRotation(v, Vector3.up);
            rot = Quaternion.AngleAxis(In("Roll").AsFloat(), v) * rot;
            rot = Program.Module.VesselInfo.OrbitalOrientation * rot;

            
            //WHY Quaternion.Euler(90, 0, 0) you ask ? BECAUSE KSP USES UP AS FORWARD, those IDIOTS
            Program.SASController.SASTarget = rot * Quaternion.Euler(90, 0, 0);
            ExecuteNext();
        }
    }
}