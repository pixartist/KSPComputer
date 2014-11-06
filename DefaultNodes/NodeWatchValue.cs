using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
namespace DefaultNodes
{
    [Serializable]
    public class NodeWatchValue : BaseExecutableNode
    {
        protected override void OnCreate()
        {
            In<Connector.Exec>("Enable");
            In<Connector.Exec>("Disable");
            Out<Connector.Exec>("OnEnable");
            Out<Connector.Exec>("OnDisable");
            In<string>("Name");
            In<string>("Value");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            if (input == In("Enable"))
            {
                KSPOperatingSystem.AddWatchedValue(this, DisplayValue);
                ExecuteNext("OnEnable");
            }
            else
            {
                OnDestroy();
                ExecuteNext("OnDisable");
            }
        }
        protected override void OnDestroy()
        {
            KSPOperatingSystem.RemoveWatchedValue(this);
        }
        private string DisplayValue()
        {
            RequestInputUpdate("Value");
            return In("Name").AsString() + ": " + In("Value").AsString();
        }
    }
}
