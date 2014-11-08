using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeGetCtrlRoll : Node
    {
        protected override void OnCreate()
        {
            Out<double>("Roll");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Roll", Vessel.ctrlState.roll);
        }
    }
}