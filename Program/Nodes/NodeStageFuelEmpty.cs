using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
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
                //Log.Write("Max fuel in stage: " + maxFuelInStage);
                if (maxFuelInStage > 0)
                {
                    double currentFuelInStage = Program.Vessel.CurrentStageFuelRemaining(DefaultResources.LiquidFuel, DefaultResources.Oxidizer, DefaultResources.SolidFuel);
                    // Log.Write("Checking for empty fuel");
                    if (currentFuelInStage <= 0)
                    {
                        //Log.Write("Fuel is empty");
                        ExecuteNext();
                    }
                }
            }
        }
    }
}