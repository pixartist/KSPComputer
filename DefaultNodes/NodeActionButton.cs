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
    public class NodeActionButton : BaseExecutableNode
    {
        protected override void OnCreate()
        {
            Out<Connector.Exec>("OnClick");
            In<string>("Name");
            
        }
        public override void OnInit()
        {
            KSPOperatingSystem.AddActionButton(OnClick, GetName);
        }
        protected override void OnDestroy()
        {
            KSPOperatingSystem.RemoveActionButton(OnClick);
        }
        private void OnClick()
        {
            ExecuteNext("OnClick");
        }
        private string GetName()
        {
            RequestInputUpdate("Name");
            return In("Name").AsString();
        }
    }
}
