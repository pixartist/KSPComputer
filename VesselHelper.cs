using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner
{
    public static class VesselHelper
    {

        public static double CurrentStageFuelRemaining(this Vessel v, params DefaultResources[] resources)
        {
            double fuel = 0;
            foreach (var part in v.Parts)
            {
                if (part.inverseStage == v.currentStage-1)
                {
                    if (part.IsUnfiredDecoupler())
                    {
                        fuel += part.CountRemainingResourcesInChildren(resources);
                    }
                }
            }
            return fuel;
        }
        public static double CurrentStageFuelMax(this Vessel v, params DefaultResources[] resources)
        {
            double maxFuel = 0;
            foreach(var part in v.Parts)
            {
                if(part.inverseStage == v.currentStage - 1)
                {
                    if (part.IsUnfiredDecoupler())
                    {
                        maxFuel += part.CountMaxResourcesInChildren(resources);
                    }
                }
            }
            return maxFuel;
        }
    }
}
