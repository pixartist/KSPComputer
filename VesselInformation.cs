using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace KSPFlightPlanner
{
    public class VesselInformation
    {
        public enum FrameOfReference
        {
            Navball
        }
        private LineRenderer up, north, east, fw;
        /// <summary>
        /// Actual up vector, center of orbited body -> center of craft (normalized)
        /// </summary>
        public Vector3 OrbitalUp { get; private set; }
        /// <summary>
        /// North Vector
        /// </summary>
        public Vector3 OrbitalNorth { get; private set; }
        /// <summary>
        /// East Vector
        /// </summary>
        public Vector3 OrbitalEast { get; private set; }
        public Vector3 Forward { get; private set; }
        public Quaternion OrbitalOrientation { get; private set; }
        public Quaternion VesselOrientation { get; private set; }

        public void Update(Vessel v)
        {
            
            var com = v.findWorldCenterOfMass();
           
            OrbitalUp = (com - v.mainBody.position).normalized;
            OrbitalNorth = v.mainBody.transform.up.normalized;
            OrbitalEast = Vector3.Cross(OrbitalUp, OrbitalNorth).normalized;
            OrbitalOrientation = Quaternion.LookRotation(OrbitalNorth, OrbitalUp);
            Forward = v.transform.up;
            VesselOrientation = v.transform.rotation;
            /*if (north == null)
            {
                up = DebugHelper.AddLine(v, Color.red);
                north = DebugHelper.AddLine(v, Color.blue);
                east = DebugHelper.AddLine(v, Color.green);
                fw = DebugHelper.AddLine(v, Color.magenta);
            }
            else
            {
                DebugHelper.UpdateLine(up, com, OrbitalUp * 500);
                DebugHelper.UpdateLine(north, com, OrbitalNorth * 500);
                DebugHelper.UpdateLine(east, com,OrbitalEast * 500);
                DebugHelper.UpdateLine(fw, com, Forward * 500);
            }*/
        }
        public Quaternion GetFrameOfReference(Vessel v, FrameOfReference reference)
        {
            switch(reference)
            {
                case FrameOfReference.Navball:
                    return OrbitalOrientation;
                default:
                    return Quaternion.identity;
            }
        }
        public Vector3 ReferenceToWorld(Vessel v, Vector3 localDirection, FrameOfReference reference)
        {
            return GetFrameOfReference(v, reference) * localDirection;
        }
        public Vector3 WorldToReference(Vessel v, Vector3 worldDirection, FrameOfReference reference)
        {
            return Quaternion.Inverse(GetFrameOfReference(v, reference)) * worldDirection;
        }
       
    }
}
