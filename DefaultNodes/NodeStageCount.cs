using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeStageCount : Node
    {
        protected override void OnCreate()
        {
            Out<double>("Stage");
        }
        protected override void OnUpdateOutputData()
        {
            Out("Stage", Staging.GetStageCount(Vessel.parts));
        }
    }
}
