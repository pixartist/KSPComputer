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
