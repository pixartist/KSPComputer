using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
using KSPComputer.Helpers;
namespace DefaultNodes
{
    [Serializable]
    public class NodeMaxThrust : Node
    {
        protected override void OnCreate()
        {
            Out<double>("Thrust");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Thrust", Program.Vessel.CurrentMaxThrust());
        }
    }
}
