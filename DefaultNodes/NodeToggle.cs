using System;
using System.Collections.Generic;

using System.Text;
using System.Threading;
using UnityEngine;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeToggle : DefaultExecutableNode
    {
        [NonSerialized]
        private bool enabled;
        protected override void OnCreate()
        {
            enabled = false;
            In<Connector.Exec>("Enable", true);
            In<Connector.Exec>("Disable", true);
            Out<Connector.Exec>("OnEnable", false);
            Out<Connector.Exec>("OnDisable", false);
        }

        protected override void OnExecute(ConnectorIn input)
        {
            if (input == In(DefaultExecName))
            {
                if (enabled)
                    ExecuteNext();
            }
            else if (!enabled)
            {
                if (input == In("Enable"))
                {
                    ExecuteNext("OnEnable");
                    enabled = true;
                }
            }
            else if (input == In("Disable"))
            {
                ExecuteNext("OnDisable");
                enabled = false;
            }
        }
    }
}