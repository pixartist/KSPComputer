﻿using System;
using System.Collections.Generic;

using System.Text;
using KSPComputer.Connectors;
namespace KSPComputer.Nodes
{
    [Serializable]
    public class SubRoutineEntry : BaseExecutableNode
    {
        public delegate void SubroutineEventHandler();
        [field: NonSerialized]
        public event SubroutineEventHandler OnRequestData;
        public void AddRoutineInput<T>(string name)
        {
            Out(name, typeof(T));
        }
        public void AddRoutineInput(string name, Type type)
        {
            Out(name, type, type != typeof(Connector.Exec));
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
