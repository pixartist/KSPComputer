using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
using KSPComputer.Helpers;
namespace DefaultNodes
{
    [Serializable]
    public class NodeStageFuelEmpty : DefaultRootNode
    {
        bool hasTriggered = false;
        protected override void OnCreate()
        {
            In<bool>("IgnoreLanded");
        }

        public override void OnUpdate()
        {
            Execute(null);
        }
        protected override void OnExecute(ConnectorIn input)
        {
            if (Vessel.currentStage > 0)
            {
                if (!Vessel.Landed || In("IgnoreLanded").AsBool())
                {
                    double maxFuelInStage = Vessel.CurrentStageFuelMax(DefaultResources.LiquidFuel, DefaultResources.Oxidizer, DefaultResources.SolidFuel);
                    if (maxFuelInStage > 0)
                    {

                        if (!Vessel.CurrentStageHasFuel())
                        {
                            if (!hasTriggered)
                            {
                                hasTriggered = true;
                                ExecuteNext();
                            }
                        }
                        else
                            hasTriggered = false;
                    }
                    else
                        hasTriggered = false;
                }
                else
                    hasTriggered = false;
            }
            else
                hasTriggered = false;
        }
    }
}