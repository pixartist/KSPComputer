using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace KSPFlightPlanner.Program
{
	[Serializable]
   
    public abstract class NodeConnectionIn
    {
		internal delegate void ValueRequestHandler();
		internal event ValueRequestHandler ValueRequest;
		private Object DataBuffer { get; set; }
        public Type DataType { get; private set; }
        public Node Owner { get; private set; }
		public NodeConnectionIn(Type dataType, Node owner)
        {
            DataType = dataType;
			Owner = owner;
        }
		internal void RequestValue()
		{
			if (ValueRequest != null)
				ValueRequest();
		}
		public double GetBufferAsDouble()
		{
			
			if(DataBuffer is double)
				return (double)DataBuffer;
			double val;
			if (double.TryParse(DataBuffer as string, out val))
				return val;
			return 0.0;
		}
        public float GetBufferAsFloat()
        {
			if (DataBuffer is float)
				return (float)DataBuffer;
            float val;
            if (float.TryParse(DataBuffer as string, out val))
                return val;
            return 0.0f;
        }
		public bool GetBufferAsBool()
		{
			if (DataBuffer is bool)
				return (bool)DataBuffer;
			bool val;
			if (bool.TryParse(DataBuffer as string, out val))
				return val;
			return false;
		}
		public SVector3 GetBufferAsVector3()
		{
			if (DataBuffer is SVector3)
				return (SVector3)DataBuffer;
			return new SVector3();
		}
    }
}
