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
    public class NodeStageFuelEmpty : RootNode
    {
        protected override void OnCreate()
        {
            In<bool>("IgnoreLanded");
            Program.OnTick += Program_OnTick;
        }

        void Program_OnTick()
        {
            Execute(null);
        }
        protected override void OnExecute(ConnectorIn input)
        {
            if (!Program.Vessel.Landed || In("IgnoreLanded").AsBool())
            {
                double maxFuelInStage = Program.Vessel.CurrentStageFuelMax(DefaultResources.LiquidFuel, DefaultResources.Oxidizer, DefaultResources.SolidFuel);
                if (maxFuelInStage > 0)
                {

                    if (!Program.Vessel.CurrentStageHasFuel())
                    {
                        ExecuteNext();
                    }
                }
            }
        }
        protected override void OnDestroy()
        {
            Program.OnTick -= Program_OnTick;
        }
    }
}