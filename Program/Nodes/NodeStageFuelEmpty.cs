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
        public new static string Name = "Stage Fuel empty";
        public new static string Description = "Called when the current stage is burned out";
        public new static SVector3 Color = new SVector3(0.2f, 0.2f, 1f);
        public new static SVector2 Size = new SVector2(210, 130);
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