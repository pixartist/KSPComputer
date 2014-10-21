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
                return program.Vessel.ActionGroups[KSPActionGroup.SAS];
            }
            set
            {
                program.Vessel.ActionGroups[KSPActionGroup.SAS] = value;
            }
        }
        public bool RCSEnabled
        {
            get
            {
                return program.Vessel.ActionGroups[KSPActionGroup.RCS];
            }
            set
            {
                program.Vessel.ActionGroups[KSPActionGroup.RCS] = value;
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
                    return program.Vessel.VesselSAS.currentRotation;
            }
            set
            {
                if (SASControlEnabled)
                    sasTarget = value;
                else
                    program.Vessel.VesselSAS.LockHeading(SASTarget, true);
            }
        }
        public bool SASControlEnabled { get; set; }
        private FlightProgram program;
        public SASController(FlightProgram program)
        {
            this.program = program;
            SASControlEnabled = true;
        }
        public void Update()
        {
            if(SASControlEnabled && SASEnabled)
            {

                Quaternion at = program.VesselInfo.VesselOrientation;
                float angle = Quaternion.Angle(at, SASTarget);
                Quaternion t = SASTarget;
                float angleAm =  Mathf.Min(1, 0.002f * (180f / angle));

                t = Quaternion.Slerp(at, t, angleAm);
                if (angle > 10f)
                    program.Vessel.VesselSAS.LockHeading(t);
                else
                    program.Vessel.VesselSAS.LockHeading(SASTarget, true);
                
            }
        }

    }
}
