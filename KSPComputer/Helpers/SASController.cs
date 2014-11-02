using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPComputer;
namespace KSPComputer.Helpers
{
    public class SASController
    {
        public bool SASEnabled
        {
            get
            {
                return VesselController.Vessel.ActionGroups[KSPActionGroup.SAS];
            }
            set
            {
                VesselController.Vessel.ActionGroups[KSPActionGroup.SAS] = value;
            }
        }
        public bool RCSEnabled
        {
            get
            {
                return VesselController.Vessel.ActionGroups[KSPActionGroup.RCS];
            }
            set
            {
                VesselController.Vessel.ActionGroups[KSPActionGroup.RCS] = value;
            }
        }
        private Quaternion sasTarget;
        public Quaternion SASTarget
        {
            get
            {
                if (SASControlEnabled)
                    return sasTarget;
                else
                    return VesselController.Vessel.VesselSAS.currentRotation;
            }
            set
            {
                if (SASControlEnabled)
                    sasTarget = value;
                else
                {
                    VesselController.Vessel.VesselSAS.LockHeading(value);
                }
            }
        }
        public bool SASControlEnabled { get; set; }
        public float SASControllerStrength { get; set; }
        public VesselController VesselController { get; private set; }
        public SASController(VesselController controller)
        {
            VesselController = controller;
            SASControlEnabled = false;
            SASControllerStrength = 1;
        }
        public void Update()
        {
            if(SASControlEnabled && SASEnabled)
            {

                Quaternion at = VesselController.VesselOrientation;
                float angle = Quaternion.Angle(at, SASTarget);
                if (angle > 10f)
                {
                    Quaternion t = SASTarget;
                    float angleAm = Mathf.Min(1, 0.005f * SASControllerStrength * (180f / angle));
                    t = Quaternion.Slerp(at, t, angleAm);

                    VesselController.Vessel.VesselSAS.LockHeading(t);
                }
                else
                    VesselController.Vessel.VesselSAS.LockHeading(SASTarget, true);
                
            }
        }

    }
}
