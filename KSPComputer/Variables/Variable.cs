using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPComputer.Variables
{
    [Serializable]
    public class Variable
    {
        public object Value { get; set; }
        public Type Type { get; private set; }
        internal Variable(Type valueType)
        {
            this.Type = valueType;
            if (this.Type.IsValueType)
                Value = Activator.CreateInstance(this.Type);
            else
                Value =  null;
        }
    }
}
