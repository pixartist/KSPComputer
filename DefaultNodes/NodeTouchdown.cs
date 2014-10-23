using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            if(!canTrigger)
            {
                canTrigger = true;
                
            }
            else if (!wasLanded)
            {
                if (Program.Vessel.Landed)
                    Execute(null);
            }
            wasLanded = Program.Vessel.Landed;
        }
    }
}