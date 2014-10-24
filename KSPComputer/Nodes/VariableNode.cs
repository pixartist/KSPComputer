using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPComputer;
using KSPComputer.Nodes;
using KSPComputer.Connectors;
using KSPComputer.Variables;
namespace KSPComputer.Nodes
{
    [Serializable]
    public class VariableNode<T> : ExecutableNode
    {
        public Variable Variable { get; private set; }
        internal void SetVariable(Variable variable)
        {
            this.Variable = variable;
        }
        protected override void OnCreate()
        {
            In<T>("Set");
            Out<T>("Get");
        }
        protected override void OnExecute(ConnectorIn input)
        {
            Variable.Value = In("Set").Get<T>();
            Log.Write(this.GetType() + " getting value " + Variable.Value + " as " + typeof(T));
            ExecuteNext();
        }
        protected override void OnUpdateOutputData()
        {
            Out("Get", Variable.Value);
        }
    }
}