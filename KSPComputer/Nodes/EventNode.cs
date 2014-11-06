using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Nodes;
namespace DefaultNodes
{
    [Serializable]
    public class EventNode : DefaultRootNode
    {
        public bool Enabled
        {
            get
            {
                RequestInputUpdate("Enabled");
                return In("Enabled").AsBool();
            }
        }
        protected override void OnCreate()
        {
            In<bool>("Enabled");
        }
    }
}