using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer;
using KSPComputer.Nodes;
namespace DefaultNodes
{
    [Serializable]
    public class NodeTouchdown : DefaultRootNode
    {
        private bool wasLanded = false;
        private bool canTrigger = false;

        public override void OnUpdate()
        {
            bool landed = Vessel.checkLanded();
            if(!canTrigger)
            {
                canTrigger = true;
            }
            else if (!wasLanded)
            {
                if (landed)
                    Execute(null);
            }
            wasLanded = landed;
        }
    }
}