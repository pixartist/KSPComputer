using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPComputer;
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
            /*Quaternion relRot = Program.VesselInfo.WorldToReference(Program.VesselInfo.VesselOrientation, VesselInformation.FrameOfReference.Navball);
            
            
            Vector3 forward = relRot * Vector3.up;
            double roll = forward.SignedAngle((Vector3.up - forward)*-1, relRot * Vector3.forward);*/
            /*Vector3 worldRight = relRot * Vector3.right;
            Log.Write("worldright: " + worldRight);
            Vector3 horizontalForward = Vector3.Cross(worldRight, Vector3.up).normalized;
            Log.Write("horizontalforward: " + horizontalForward);
            Vector3 verticalUp = Vector3.Cross(horizontalForward, worldRight).normalized;
            Log.Write("verticalUp: " + verticalUp);
            double roll = -horizontalForward.SignedAngle(verticalUp, Vector3.up);
            Log.Write("Roll: " + roll);*/
            //var h = Program.Module.VesselInfo.Forward;
            Out("N/S", (double)Program.VesselInfo.NavballHeading.z);
            Out("E/W", (double)Program.VesselInfo.NavballHeading.x);
            Out("U/D", (double)Program.VesselInfo.NavballHeading.y);
            Out("Roll", Program.VesselInfo.Roll);
        }
    }
}
