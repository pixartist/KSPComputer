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
    public class NodeValueReached : EventNode
    {
        private double lastValue;
        private bool canTrigger;
        private bool wasDown;
        protected override void OnCreate()
        {
            base.OnCreate();
            In<double>("Value");
            In<double>("TriggerAt");
            In<bool>("Down");
            lastValue = 0;
            canTrigger = false;
            wasDown = false;
        }

        public override void OnUpdate()
        {
            if(Enabled)
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
    }
}