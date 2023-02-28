using System;
using System.Collections.Generic;

using System.Text;
using KSPComputer;
using KSPComputer.Nodes;
namespace DefaultNodes
{
    [Serializable]
    public class NodePreLaunch : DefaultRootNode
    {


        public override void OnLaunch()
        {
            Execute(null);
        }
    }
}
