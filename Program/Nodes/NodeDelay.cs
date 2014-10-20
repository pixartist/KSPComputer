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
        const int maxEntries = 32;
        protected override void OnCreate()
        {
            In<double>("Delay");
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
                double d = In("Delay").AsDouble();
                triggerTimes.Add((float)(Time.time + d));
                Out("Active", true);
            }
            else
            {
                throw (new Exception(this.GetType() + ": Entry count exceeded!"));
            }
        }
  
    }
}