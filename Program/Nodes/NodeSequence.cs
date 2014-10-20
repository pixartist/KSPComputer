using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program.Connectors;
namespace KSPFlightPlanner.Program.Nodes
{
    [Serializable]
    public class NodeSequence : ExecutableNode
    {
        public new static string Name = "Sequence";
        public new static string Description = "Executes multiple outputs";
        public new static SVector3 Color = new SVector3(1f, 1f, 1f);
        public new static SVector2 Size = new SVector2(190, 200);
        protected override void OnCreate()
        {
            Out<Connector.Exec>(DefaultExecName + "2", false);
            Out<Connector.Exec>(DefaultExecName + "3", false);
            Out<Connector.Exec>(DefaultExecName + "4", false);
        }
        protected override void OnExecute(ConnectorIn input)
        {
            ExecuteNext();
            ExecuteNext(DefaultExecName + "2");
            ExecuteNext(DefaultExecName + "3");
            ExecuteNext(DefaultExecName + "4");

        }
    }
}
