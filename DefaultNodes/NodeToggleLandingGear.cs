using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeToggleLandingGear : DefaultExecutableNode
    {
        protected override void OnCreate()
        {
            In<bool>("On");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            var on = In("On").AsBool();
            Vessel.ActionGroups.SetGroup(KSPActionGroup.Gear, on);
            ExecuteNext();
        }
    }
}