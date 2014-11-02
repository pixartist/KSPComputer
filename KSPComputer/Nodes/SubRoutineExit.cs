using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPComputer.Connectors;
namespace KSPComputer.Nodes
{
    [Serializable]
    public class SubRoutineExit : BaseExecutableNode
    {
        public delegate void SubroutineEventHandler(string node);
        [field: NonSerialized]
        public event SubroutineEventHandler OnExecuted;
        public void AddRoutineOuput<T>(string name)
        {
            In<T>(name);
        }
        public void AddRoutineOuput(string name, Type type)
        {
            In(name, type);
        }
        public void RemoveRoutineOutput(string name)
        {
            base.RemoveInput(name);
        }
        protected override void OnExecute(ConnectorIn input)
        {
            if (OnExecuted != null)
                OnExecuted(input.Name);
        }
    }
}
