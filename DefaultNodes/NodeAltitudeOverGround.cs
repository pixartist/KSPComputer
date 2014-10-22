using System;
using System.Collections.Generic;
using System.Linq;
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
            double a = Program.Vessel.heightFromTerrain;
            if(a < 0)
                a = Program.Vessel.altitude;
            Out("Altitude", a);
        }
    }
}
