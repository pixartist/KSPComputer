using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeTimeToApoapsis : Node
    {
        protected override void OnCreate()
        {
            Out<double>("Seconds");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Seconds", Vessel.orbit.timeToAp);
        }
    }
}
