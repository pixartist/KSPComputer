using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeAltitude : Node
    {
        protected override void OnCreate()
        {
            Out<double>("Altitude");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Altitude", Program.Vessel.altitude);
        }
    }
}
