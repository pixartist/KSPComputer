using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeGetActionGroup6 : RootNode
    {
        protected override void OnCreate()
        {
            Out<bool>("State");
            Program.OnTick += Program_OnTick;
        }
        protected override void OnExecute(ConnectorIn input)
        {
            Out("State", Program.Vessel.ActionGroups[KSPActionGroup.Custom06]);
            ExecuteNext();
        }

        void Program_OnTick()
        {
            Execute(null);
        }

        protected override void OnDestroy()
        {
            Program.OnTick -= Program_OnTick;
        }
    }
}