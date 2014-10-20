using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeNavballHeading : Node
    {
        protected override void OnCreate()
        {
            Out<double>("N/S");
            Out<double>("E/W");
            Out<double>("U/D");
            Out<double>("Roll");
        }
        protected override void OnUpdateOutputData()
        {
            Quaternion relRot = (Quaternion.Inverse(Program.Module.VesselInfo.OrbitalOrientation) * Program.Module.VesselInfo.VesselOrientation) * Quaternion.Inverse(Quaternion.Euler(90, 0, 0));
            var h = Program.Module.VesselInfo.WorldToReference(
                Program.Vessel,
                Program.Module.VesselInfo.Forward,
                VesselInformation.FrameOfReference.Navball);
            Vector3 dir = relRot * Vector3.forward;
            Vector3 up = relRot * Vector3.up;
            double roll = Mathf.Atan2(
                Vector3.Dot(dir, Vector3.Cross(Vector3.up, up)),
                Vector3.Dot(Vector3.up, up)) * Mathf.Rad2Deg;

            //var h = Program.Module.VesselInfo.Forward;
            Out("N/S", (double)dir.z);
            Out("E/W", (double)dir.x);
            Out("U/D", (double)dir.y);
            Out("Roll", roll);
        }
    }
}
