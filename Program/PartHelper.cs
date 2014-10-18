using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPFlightPlanner.Program
{
    public static class PartHelper
    {
        public static double CountMaxResources(this Part part, params DefaultResources[] resources)
        {
            return (from PartResource r in part.Resources where resources.Contains((DefaultResources)r.info.id) select r.maxAmount).Sum();
        }
        public static double CountRemainingResources(this Part part, params DefaultResources[] resources)
        {
            return (from PartResource r in part.Resources where resources.Contains((DefaultResources)r.info.id) select r.amount).Sum();
        }
        public static double CountMaxResourcesInChildren(this Part part, params DefaultResources[] resources)
        {
            return CountMaxResources(part, resources) + (from Part p in part.children select p.CountMaxResourcesInChildren(resources)).Sum();
        }
        public static double CountRemainingResourcesInChildren(this Part part, params DefaultResources[] resources)
        {
            return CountRemainingResources(part, resources) + (from Part p in part.children select p.CountRemainingResourcesInChildren(resources)).Sum();
        }
        
        public static bool IsUnfiredDecoupler(this Part part)
        {
            foreach (PartModule m in part.Modules)
            {
                ModuleDecouple mDecouple = m as ModuleDecouple;
                if (mDecouple != null)
                {
                    if (!mDecouple.isDecoupled) 
                        return true;
                    break;
                }
                ModuleAnchoredDecoupler mAnchoredDecoupler = m as ModuleAnchoredDecoupler;
                if (mAnchoredDecoupler != null)
                {
                    if (!mAnchoredDecoupler.isDecoupled) 
                        return true;
                    break;
                }
            }
            return false;
        }
    }
}
