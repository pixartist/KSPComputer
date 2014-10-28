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
    public class NodeMass : Node
    {
        protected override void OnCreate()
        {
            Out<double>("Mass");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Mass", Program.Vessel.GetTotalMass());
        }
    }
}
