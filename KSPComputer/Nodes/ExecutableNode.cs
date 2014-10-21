using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPComputer.Connectors;
namespace KSPComputer.Nodes
{
    [Serializable]
    public abstract class ExecutableNode : RootNode
    {
        public ExecutableNode()
            : base()
        {
            In<Connector.Exec>(DefaultExecName, true);
        }
    }
}