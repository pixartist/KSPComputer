using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeTick : RootNode
    {
        protected override void OnCreate()
        {
            Program.OnTick += Program_OnTick;
        }

        void Program_OnTick()
        {
            Execute(null);
        }
    }
}