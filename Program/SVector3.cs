using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace KSPFlightPlanner.Program
{
    [Serializable]
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
        public Color GetColor()
        {
            return new Color(x, y, z);
        }
        public override string ToString()
        {
            return String.Format("SVector3: {0}, {1}, {2}", x, y, z);
        }
    }
}