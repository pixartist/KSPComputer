using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
using KSPComputer.Helpers;
namespace DefaultNodes
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
            Quaternion relRot = Program.VesselInfo.WorldToReference(Program.VesselInfo.VesselOrientation, VesselInformation.FrameOfReference.Navball);
            Vector3 absUp = (Vector3.forward * -1f);
            Vector3 dir = relRot * Vector3.up;
            Vector3 up = relRot * absUp;
            double roll = Mathf.Atan2(
                Vector3.Dot(dir, Vector3.Cross(absUp, up)),
                Vector3.Dot(absUp, up)) * Mathf.Rad2Deg;

            //var h = Program.Module.VesselInfo.Forward;
            Out("N/S", (double)dir.z);
            Out("E/W", (double)dir.x);
            Out("U/D", (double)dir.y);
            Out("Roll", roll);
        }
    }
}
