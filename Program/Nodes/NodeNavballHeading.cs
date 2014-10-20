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
        public new static string Name = "Navball heading";
        public new static string Description = "Returns the current navball heading";
        public new static SVector3 Color = new SVector3(0.5f, 1f, 0.5f);
        public new static SVector2 Size = new SVector2(190, 120);
        protected override void OnCreate()
        {
            Out<float>("N/S");
            Out<float>("E/W");
            Out<float>("U/D");
            Out<float>("Roll");
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
            float roll = Mathf.Atan2(
                Vector3.Dot(dir, Vector3.Cross(Vector3.up, up)),
                Vector3.Dot(Vector3.up, up)) * Mathf.Rad2Deg;

            //var h = Program.Module.VesselInfo.Forward;
            Out("N/S", dir.z);
            Out("E/W", dir.x);
            Out("U/D", dir.y);
            Out("Roll", roll);
        }
    }
}
