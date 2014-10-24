using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeValueReached : RootNode
    {
        private double lastValue;
        private bool canTrigger;
        private bool wasDown;
        protected override void OnCreate()
        {
            In<double>("Value");
            In<double>("TriggerAt");
            In<bool>("Down");
            Program.OnTick += Program_OnTick;
            lastValue = 0;
            canTrigger = false;
            wasDown = false;
        }

        void Program_OnTick()
        {
            Execute(null);
        }
        protected override void OnExecute(ConnectorIn input)
        {
            bool down = In("Down").AsBool();
            if(down != wasDown)
            {
                wasDown = down;
                canTrigger = false;
                
            }
            double v = In("Value").AsDouble();
            if (canTrigger)
            {
                double t = In("TriggerAt").AsDouble();
                if (down)
                {
                    if (lastValue > t)
                    {
                        if (v <= t)
                        {
                            ExecuteNext();
                        }
                    }
                }
                else
                {
                    if (lastValue < t)
                    {
                        if (v >= t)
                        {
                            ExecuteNext();
                        }
                    }
                }
            }
            else
            {
                canTrigger = true;
            }
            lastValue = v;
        }
        protected override void OnDestroy()
        {
            Program.OnTick -= Program_OnTick;
        }
    }
}