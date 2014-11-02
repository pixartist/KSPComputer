using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer.Connectors;
namespace KSPComputer.Nodes
{
    [Serializable]
    public class SubRoutineEntry : BaseExecutableNode
    {
        public delegate void SubroutineEventHandler();
        public event SubroutineEventHandler OnRequestData;
        public void AddRoutineInput<T>(string name)
        {
            Out<T>(name);
        }
        public void AddRoutineInput(string name, Type type)
        {
            Out(name, type);
        }
        public void RemoveRoutineInput(string name)
        {
            base.RemoveOutput(name);
        }
        protected override void OnExecute(ConnectorIn input)
        { 
            ExecuteNext(input.Name); 
        }

        protected override void OnUpdateOutputData()
        {
            if (OnRequestData != null)
                OnRequestData();
        }
    }
}
