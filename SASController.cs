using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPFlightPlanner.Program;
using UnityEngine;
namespace KSPFlightPlanner
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
        public Quaternion SASTarget { get; set; }
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
                
                Quaternion at = program.Vessel.transform.rotation;
                float angle = Quaternion.Angle(at, SASTarget);
                if (angle > 10)
                    program.Vessel.VesselSAS.LockHeading(SASTarget);
                else   
                    program.Vessel.VesselSAS.LockHeading(SASTarget, true);
                
            }
        }

    }
}
