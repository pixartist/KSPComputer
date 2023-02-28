using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;
namespace KSPComputer.Types
{
    [Serializable]
    public struct SQuaternion
    {
        public float x, y, z, w;
        public SQuaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        public SQuaternion(Quaternion q)
        {
            x = q.x;
            y = q.y;
            z = q.z;
            w = q.w;
        }
        public Quaternion GetQuaternion()
        {
            return new Quaternion(x, y, z, w);
        }
        public override string ToString()
        {
            return String.Format("SQuaternion: {0}, {1}, {2}, {3}", x, y, z, w);
        }
    }
}