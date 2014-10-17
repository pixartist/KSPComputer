using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeTick : RootNode
    {
        public new static string Name = "Tick";
        public new static string Description = "Called every update";
        public new static SVector3 Color = new SVector3(1, 0.2f, 0.2f);
        public new static SVector2 Size = new SVector2(150, 50);
        protected override void OnCreate()
        {
            Program.OnTick += Program_OnTick;
        }

        void Program_OnTick()
        {
            Execute();
        }
    }
}