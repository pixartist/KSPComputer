using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeBranch : ExecutableNode
    {
        public new static string Name = "Branch";
        public new static string Description = "If/Else";
        public new static SVector3 Color = new SVector3(1f, 1f, 1f);
        public new static SVector2 Size = new SVector2(190, 200);
        protected override void OnCreate()
        {
            In<bool>("Input");
            Out<Connector.Exec>("True", false);
            Out<Connector.Exec>("False", false);
        }
        protected override void OnExecute()
        {
            bool isTrue = In("Input").AsBool();
            Log.Write("Checking branch: " + isTrue);
            ExecuteNext();
            if(In("Input").AsBool())
                ExecuteNext("True");
            else
                ExecuteNext("False");
        }
    }
}
