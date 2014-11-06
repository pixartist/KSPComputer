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
        public static bool CurrentStageHasFuel(this Vessel v)
        {
            if (v.currentStage > 0)
            {
                foreach (var part in v.Parts)
                {
                    if (part.inverseStage == v.currentStage - 1)
                    {
                        if (part.IsUnfiredDecoupler())
                        {
                            if (part.CheckEnginesHaveFuelInChildren())
                                return true;
                        }
                    }
                }
            }
            else
            {
                if (v.rootPart.CheckEnginesHaveFuelInChildren())
                    return true;
            }
            return false;
        }
        public static double CurrentStageFuelMax(this Vessel v, params DefaultResources[] resources)
        {
            double maxFuel = 0;
            if (v.currentStage > 0)
            {
                //Log.Write("Checking stage parts");
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
               // Log.Write("Checking all parts");
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
                        if (part.IsUnfiredDecoupler() )
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
