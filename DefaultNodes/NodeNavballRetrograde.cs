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
    public class NodeNavballRetrograde : Node
    {
        protected override void OnCreate()
        {
            Out<double>("N/S");
            Out<double>("E/W");
            Out<double>("U/D");
        }
        protected override void OnUpdateOutputData()
        {
            Vector3 dir = Program.VesselInfo.WorldToReference(Program.VesselInfo.Prograde*-1f, VesselInformation.FrameOfReference.Navball);
            //Vector3 up = relRot * Vector3.up;

            //var h = Program.Module.VesselInfo.Forward;
            Out("N/S", (double)dir.z);
            Out("E/W", (double)dir.x);
            Out("U/D", (double)dir.y);
        }
    }
}
