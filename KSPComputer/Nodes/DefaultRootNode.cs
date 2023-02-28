using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;
using KSPComputer.Connectors;
namespace KSPComputer.Nodes
{
    [Serializable]
    public abstract class DefaultRootNode : BaseExecutableNode
    {
        public DefaultRootNode()
            : base()
        {

            Out<Connector.Exec>(DefaultExecName, false);
        } 
    }
}