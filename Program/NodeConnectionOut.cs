using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program
{
	[Serializable]
    public abstract class NodeConnectionOut
    {
		private Object DataBuffer { get; set; }
		private bool DataRequested
        public Type DataType { get; private set; }
        public bool MultiOutput { get; private set; }
		
        public Node Owner { get; private set; }
        public NodeConnectionIn ConnectedTo { get; private set; }
		public NodeConnectionOut(Type dataType, Node owner, bool multiOutput)
        {
            DataType = dataType;
            MultiOutput = multiOutput;
			Owner = owner;
        }
		public void Connect(NodeConnectionIn to)
		{
			if(ConnectedTo != null)
				ConnectedTo.ValueRequest -= ConnectedTo_ValueRequest;
			ConnectedTo = to;
			if(ConnectedTo != null)
				ConnectedTo.ValueRequest += ConnectedTo_ValueRequest;
		}
		void ConnectedTo_ValueRequest()
		{
			throw new NotImplementedException();
		}

    }
}
