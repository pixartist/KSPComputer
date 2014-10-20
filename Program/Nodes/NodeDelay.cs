using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeDelay : ExecutableNode
    {
        private List<float> triggerTimes;
        public new static string Name = "Delay (s)";
        public new static string Description = "Delays incoming executions";
        public new static SVector3 Color = new SVector3(1, 1, 1);
        public new static SVector2 Size = new SVector2(150, 100);
        const int maxEntries = 32;
        protected override void OnCreate()
        {
            In<float>("Delay");
            Out<bool>("Active");
            triggerTimes = new List<float>();
            Program.OnTick += Program_OnTick;
        }

        void Program_OnTick()
        {
            if (triggerTimes.Count > 0)
            {
                bool removed = false;
                float time = Time.time;
                int i = 0;
                for (i = 0; i < triggerTimes.Count; i++)
                {
                    if (triggerTimes[i] <= time)
                    {
                        removed = true;
                        break;
                    }
                }
                if (removed)
                {
                    triggerTimes.RemoveAt(i);
                    ExecuteNext();
                    if (triggerTimes.Count < 1)
                        Out("Active", false);
                }
            }
        }
        protected override void OnExecute(ConnectorIn input)
        {
            if (triggerTimes.Count < maxEntries)
            {
                float d = In("Delay").AsFloat();
                triggerTimes.Add(Time.time + d);
                Out("Active", true);
            }
            else
            {
                throw (new Exception(this.GetType() + ": Entry count exceeded!"));
            }
        }
  
    }
}