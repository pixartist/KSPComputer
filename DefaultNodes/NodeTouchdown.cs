using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer;
using KSPComputer.Nodes;
namespace DefaultNodes
{
    [Serializable]
    public class NodeTouchdown : RootNode
    {
        private bool wasLanded = false;
        private bool canTrigger = false;
        protected override void OnCreate()
        {
            Program.OnTick += Program_OnTick;
        }

        void Program_OnTick()
        {
            bool landed = Program.Vessel.checkLanded();
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
        protected override void OnDestroy()
        {
            Program.OnTick -= Program_OnTick;
        }
    }
}