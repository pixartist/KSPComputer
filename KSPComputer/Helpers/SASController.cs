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
        private Quaternion sasTarget;
        public Quaternion SASTarget {
            get {
                return sasTarget;
            }
            set {
                sasTarget = value;
            }
        }
        public float SASControllerStrength { get; set; }
        public VesselController VesselController { get; private set; }
        public SASController(VesselController controller) {
            VesselController = controller;
            SASControllerStrength = 1;
        }
        public void Update() {
            if (SASEnabled) {

                Quaternion at = VesselController.VesselOrientation;
                float angle = Quaternion.Angle(at, SASTarget);
                if (angle > 10f) {
                    Quaternion t = SASTarget;
                    float angleAm = Mathf.Min(1, 0.005f * SASControllerStrength * (180f / angle));
                    t = Quaternion.Slerp(at, t, angleAm);

                    VesselController.Vessel.Autopilot.SAS.SetTargetOrientation(t.eulerAngles, true);
                } else
                    VesselController.Vessel.Autopilot.SAS.SetTargetOrientation(SASTarget.eulerAngles, false);

            }
        }

    }
}
