using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodePreLaunch : RootNode
    {
        protected override void OnCreate()
        {
            Program.OnLaunch += Program_OnLaunch;
        }

        void Program_OnLaunch()
        {
            Execute(null);
        }
    }
}
