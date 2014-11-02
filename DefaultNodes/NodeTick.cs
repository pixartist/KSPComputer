using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
namespace DefaultNodes
{
    [Serializable]
    public class NodeTick : DefaultRootNode
    {
        public override void OnUpdate()
        {
            Execute(null);
        }
    }
}