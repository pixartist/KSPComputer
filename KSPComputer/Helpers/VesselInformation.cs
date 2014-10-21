using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace KSPComputer.Helpers
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
        public Vector3 Prograde { get; private set; }
        private FlightProgram program;
        public VesselInformation(FlightProgram program)
        {
            this.program = program;
        }
        public void Update()
        {

            var com = program.Vessel.findWorldCenterOfMass();

            OrbitalUp = (com - program.Vessel.mainBody.position).normalized;
            OrbitalNorth = program.Vessel.mainBody.transform.up.normalized;
            OrbitalEast = Vector3.Cross(OrbitalUp, OrbitalNorth).normalized;
            OrbitalOrientation = Quaternion.LookRotation(OrbitalNorth, OrbitalUp);
            Forward = program.Vessel.transform.up;
            VesselOrientation = program.Vessel.transform.rotation;
            //if(program.Vessel.at)
            Prograde = program.Vessel.srf_velocity.normalized;
            if (north == null)
            {
                //up = DebugHelper.AddLine(v, Color.red);
                north = DebugHelper.AddLine(program.Vessel, Color.blue);
                //east = DebugHelper.AddLine(v, Color.green);
                //fw = DebugHelper.AddLine(v, Color.magenta);
            }
            else
            {
               // DebugHelper.UpdateLine(up, com, OrbitalUp * 500);
                DebugHelper.UpdateLine(north, com, Prograde * 500);
               // DebugHelper.UpdateLine(east, com,OrbitalEast * 500);
                //DebugHelper.UpdateLine(fw, com, Forward * 500);
            }
        }
        public Quaternion ReferenceToWorld(Quaternion localRotation, FrameOfReference reference)
        {
            switch (reference)
            {
                case FrameOfReference.Navball:
                    return OrbitalOrientation * localRotation ;
                default:
                    return Quaternion.identity;
            }
        }
        public Quaternion WorldToReference(Quaternion worldRotation, FrameOfReference reference)
        {
            switch (reference)
            {
                case FrameOfReference.Navball:
                    return Quaternion.Inverse(OrbitalOrientation) * worldRotation ;
                default:
                    return Quaternion.identity;
            }
        }
        public Vector3 ReferenceToWorld(Vector3 localDirection, FrameOfReference reference)
        {
            switch (reference)
            {
                case FrameOfReference.Navball:
                    return (OrbitalOrientation * localDirection);
                default:
                    return Vector3.zero;
            }
        }
        public Vector3 WorldToReference(Vector3 worldDirection, FrameOfReference reference)
        {
            switch (reference)
            {
                case FrameOfReference.Navball:
                    return (Quaternion.Inverse(OrbitalOrientation) * worldDirection);
                default:
                    return Vector3.zero;
            }
        }
    }
}
