using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeSetActionGroup3 : ExecutableNode
    {
        protected override void OnCreate()
        {
            In<bool>("State");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            var state = In("State").AsBool();
            Program.Vessel.ActionGroups.SetGroup(KSPActionGroup.Custom03, state);
            ExecuteNext();
        }
    }
}