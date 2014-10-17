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
            AddConnectorIn("IgnoreLanded", new BoolConnectorIn());
            Program.OnTick += Program_OnTick;
        }

        void Program_OnTick()
        {
            Execute();
        }
        protected override void OnExecute()
        {
            if (!Program.Vessel.Landed || GetConnectorIn("IgnoreLanded").GetBufferAsBool())
            {
               // Log.Write("Checking for empty fuel");
                if (Program.Vessel.CanSavelySeperateCurrentStage())
                {
                    //Log.Write("Fuel is empty");
                    ExecuteNext();
                }
            }
        }
    }
}