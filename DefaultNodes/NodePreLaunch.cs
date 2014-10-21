using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
namespace DefaultNodes
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
