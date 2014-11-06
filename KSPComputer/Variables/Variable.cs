using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPComputer.Variables
{
    [Serializable]
    public class Variable
    {
        [NonSerialized]
        private object value;
        public object Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }
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
