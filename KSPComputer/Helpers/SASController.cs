using System;
using UnityEngine;
namespace KSPComputer.Helpers {
    public class SASController {
        public bool SASEnabled {
            get {
                return VesselController.Vessel.ActionGroups[KSPActionGroup.SAS];
            }
            set {
                Log.Write("Setting SAS to " + value);
                VesselController.Vessel.ActionGroups[KSPActionGroup.SAS] = value;
            }
        }
        public bool RCSEnabled {
            get {
                return VesselController.Vessel.ActionGroups[KSPActionGroup.RCS];
            }
            set {
                VesselController.Vessel.ActionGroups[KSPActionGroup.RCS] = value;
            }
        }
        public VesselController VesselController { get; private set; }
        public SASController(VesselController controller) {
            VesselController = controller;
        }
        public void SetSASTarget(double lat, double lon) {
            var v = new Vector3d(Math.Sin(lon), 0, Math.Cos(lon));
            v = Quaternion.AngleAxis((float)lat, new Vector3d(-v.z, 0, v.x)) * v;
            var t = VesselController.ReferenceToWorld(v, VesselController.FrameOfReference.Navball);
            SASEnabled = true;
            VesselController.Vessel.Autopilot.SetMode(VesselAutopilot.AutopilotMode.StabilityAssist);
            VesselController.Vessel.Autopilot.SAS.lockedMode = false;
            VesselController.Vessel.Autopilot.SAS.SetTargetOrientation(t, false);
        }
        public void SetSASTarget(Quaternion orientation) {
            var t = VesselController.ReferenceToWorld(orientation, VesselController.FrameOfReference.Navball);
            SASEnabled = true;
            VesselController.Vessel.Autopilot.SetMode(VesselAutopilot.AutopilotMode.StabilityAssist);
            VesselController.Vessel.Autopilot.SAS.lockedMode = false;
            VesselController.Vessel.Autopilot.SAS.LockRotation(t);
        }
    }
}
