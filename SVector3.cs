using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace KSPFlightPlanner
{
	public struct SVector3
	{
		public float x, y, z;
		public SVector3(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
		public Vector3 GetVec3()
		{
			return new Vector3(x, y, z);
		}
	}
}
