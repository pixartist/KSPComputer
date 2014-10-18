using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeToggleSAS : ExecutableNode
    {
        public new static string Name = "Toggle SAS";
        public new static string Description = "Enables or disables the SAS";
        public new static SVector3 Color = new SVector3(1, 1, 0.2f);
        public new static SVector2 Size = new SVector2(150, 100);
        protected override void OnCreate()
        {
            In<bool>("On");
        }
        protected override void OnExecute()
        {
            var on = In("On").AsBool();
            Program.SASController.SASEnabled = on;
            ExecuteNext();
        }
    }
}