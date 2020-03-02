using UnityEngine;
namespace KSPComputer.Helpers {
    public class VesselController {
        public enum FrameOfReference {
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
        public Vessel Vessel { get; private set; }
        public SASController SASController { get; private set; }
        public Vector3 Velocity { get; private set; }
        public Vector3 Heading { get; private set; }
        public Vector3 WorldPosition { get; private set; }
        public Quaternion OrbitalOrientation { get; private set; }
        public Quaternion VesselOrientation { get; private set; }
        public Quaternion NavballOrientation { get; private set; }
        public Vector3 NavballHeading { get; private set; }
        public Vector3 GravityVector { get; private set; }
        public Vector3 CenterOfMass { get; private set; }
        public double CurrentGravity { get; private set; }
        public float G {
            get {
                return 6.674E-11f;
            }
        }

        public double Roll { get; private set; }
        public Vector3 Prograde { get; private set; }
        public bool InOrbit { get; private set; }
        public VesselController(Vessel vessel) {
            this.Vessel = vessel;
            this.SASController = new SASController(this);
        }
        public void Update() {
            InOrbit = Vessel.altitude > TimeWarp.fetch.GetAltitudeLimit(5, Vessel.mainBody);
            Velocity = InOrbit ? Vessel.obt_velocity : Vessel.srf_velocity;
            CenterOfMass = Vessel.CoM;
            WorldPosition = Vessel.transform.position;
            OrbitalUp = (CenterOfMass - Vessel.mainBody.position).normalized;
            OrbitalNorth = Vessel.mainBody.transform.up.normalized;
            OrbitalEast = Vector3.Cross(OrbitalUp, OrbitalNorth).normalized;
            OrbitalOrientation = Quaternion.LookRotation(OrbitalNorth, OrbitalUp);
            Heading = Vessel.transform.up;
            VesselOrientation = Vessel.transform.rotation;
            NavballOrientation = WorldToReference(VesselOrientation, FrameOfReference.Navball);
            NavballHeading = NavballOrientation * Vector3.up;
            Roll = NavballHeading.SignedAngle((Vector3.up - NavballHeading) * -1, NavballOrientation * Vector3.forward);
            //if(program.Vessel.at)
            Prograde = Velocity.normalized;

            GravityVector = FlightGlobals.getGeeForceAtPosition(CenterOfMass);
            CurrentGravity = GravityVector.magnitude;
            /*  if (north == null)
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
              }*/
            SASController.Update();
        }
        public Quaternion ReferenceToWorld(Quaternion localRotation, FrameOfReference reference) {
            switch (reference) {
                case FrameOfReference.Navball:
                    return OrbitalOrientation * localRotation;
                default:
                    return Quaternion.identity;
            }
        }
        public Quaternion WorldToReference(Quaternion worldRotation, FrameOfReference reference) {
            switch (reference) {
                case FrameOfReference.Navball:
                    return Quaternion.Inverse(OrbitalOrientation) * worldRotation;
                default:
                    return Quaternion.identity;
            }
        }
        public Vector3d ReferenceToWorld(Vector3d localDirection, FrameOfReference reference) {
            switch (reference) {
                case FrameOfReference.Navball:
                    return (OrbitalOrientation * localDirection);
                default:
                    return Vector3d.zero;
            }
        }
        public Vector3d WorldToReference(Vector3d worldDirection, FrameOfReference reference) {
            switch (reference) {
                case FrameOfReference.Navball:
                    return (Quaternion.Inverse(OrbitalOrientation) * worldDirection);
                default:
                    return Vector3d.zero;
            }
        }
        public Vector3d GeoToWorld(Vector3d geo) {
            return Vessel.mainBody.GetWorldSurfacePosition(geo.z, geo.x, geo.y);
        }
        public Vector3d WorldToGeo(Vector3d world) {
            return new Vector3d(Vessel.mainBody.GetLongitude(world), Vessel.mainBody.GetAltitude(world), Vessel.mainBody.GetLatitude(world));
        }
    }
}
