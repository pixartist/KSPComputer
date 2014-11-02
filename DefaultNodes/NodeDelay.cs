using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeDelay : DefaultExecutableNode
    {
        private List<double> triggerTimes;
        const int maxEntries = 128;
        protected override void OnCreate()
        {
            In<double>("Delay");
            Out<bool>("Active");
            triggerTimes = new List<double>();
        }

        public override void OnUpdate()
        {
            if (triggerTimes.Count > 0)
            {
                bool removed = false;
                double time = Planetarium.GetUniversalTime();
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
                triggerTimes.Add(Planetarium.GetUniversalTime() + d);
                Out("Active", true);
            }
        }
    }
}