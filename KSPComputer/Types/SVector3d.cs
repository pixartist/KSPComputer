using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace KSPComputer.Types
{
    [Serializable]
    public struct SVector3d
    {
        public double x, y, z;
        public SVector3d(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public SVector3d(Vector3d v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }
        public Vector3d GetVec3()
        {
            return new Vector3d(x, y, z);
        }
        public Color GetColor()
        {
            return new Color((float)x, (float)y, (float)z);
        }
        public override string ToString()
        {
            return String.Format("SVector3: {0}, {1}, {2}", x, y, z);
        }
    }
}