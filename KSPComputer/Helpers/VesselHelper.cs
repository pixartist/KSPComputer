using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPComputer.Helpers
{
    public static class VesselHelper
    {

        public static double CurrentStageFuelRemaining(this Vessel v, params DefaultResources[] resources)
        {
            double fuel = 0;
            if (v.currentStage > 0)
            {
                foreach (var part in v.Parts)
                {
                    if (part.inverseStage == v.currentStage - 1)
                    {
                        if (part.IsUnfiredDecoupler())
                        {
                            fuel += part.CountRemainingResourcesInChildren(resources);
                        }
                    }
                }
            }
            else
            {
                fuel += v.rootPart.CountRemainingResourcesInChildren(resources);
            }
            return fuel;
        }
        public static double CurrentStageFuelMax(this Vessel v, params DefaultResources[] resources)
        {
            double maxFuel = 0;
            if (v.currentStage > 0)
            {
                foreach (var part in v.Parts)
                {
                    if (part.inverseStage == v.currentStage - 1)
                    {
                        if (part.IsUnfiredDecoupler())
                        {
                            maxFuel += part.CountMaxResourcesInChildren(resources);
                        }
                    }
                }
            }
            else
            {
                maxFuel += v.rootPart.CountMaxResourcesInChildren(resources);
            }
            return maxFuel;
        }
        public static double CurrentMaxThrust(this Vessel v)
        {
            double thrust = 0;
            if (v.currentStage > 0)
            {
                foreach (var part in v.Parts)
                {
                    if (part.inverseStage == v.currentStage - 1)
                    {
                        if (part.IsUnfiredDecoupler())
                        {
                            thrust += part.CountMaxThrustInChildren();
                        }
                    }
                }
            }
            else
            {
                thrust += v.rootPart.CountMaxThrustInChildren();
            }
            return thrust;
        }
    }
}
