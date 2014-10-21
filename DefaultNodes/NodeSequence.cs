using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
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
