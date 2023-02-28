using System;
using System.Collections.Generic;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeAltitudeOverGround : Node
    {
        protected override void OnCreate()
        {
            Out<double>("Altitude");
        }
        protected override void OnUpdateOutputData()
        {
            double a = Math.Min(Vessel.heightFromTerrain, Vessel.altitude);
            
            Out("Altitude", a);
        }
    }
}
