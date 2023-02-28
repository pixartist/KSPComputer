

namespace KSPComputer.Helpers {
    public static class PartHelper {
        public static double CountMaxResources(this Part part, params DefaultResources[] resources) {
            if (part.IsUnfiredDecoupler() || part.IsSepratron())
                return 0.0;
            double sum = 0;
            foreach(var r in part.Resources) {
                if(resources.IndexOf((DefaultResources)r.info.id) >= 0) {
                    sum += r.maxAmount;
                }
            }
            return sum;
        }
        public static double CountRemainingResources(this Part part, params DefaultResources[] resources) {
            if (part.IsUnfiredDecoupler() || part.IsSepratron())
                return 0.0;
            double sum = 0;
            foreach(var r in part.Resources) {
                if(resources.IndexOf((DefaultResources)r.info.id) >= 0 && r.amount > 0.01) {
                    sum += r.maxAmount;
                }
            }
            return sum;
        }
        public static bool CheckEngineHasFuel(this Part part) {
            if (!part.IsEngine())
                return false;
            else if (part.IsSepratron())
                return false;
            else {
                foreach (PartModule m in part.Modules) {
                    ModuleEngines eng = m as ModuleEngines;
                    if (eng != null) return !eng.getFlameoutState;

                    ModuleEnginesFX engFX = m as ModuleEnginesFX;
                    if (engFX != null) return !engFX.getFlameoutState;
                }
            }
            return false;
        }
        public static float GetMaxThrust(this Part part) {
            float t = 0;
            foreach (PartModule pm in part.Modules) {
                if (pm is ModuleEngines)
                    t += (pm as ModuleEngines).maxThrust;
                if (pm is ModuleEnginesFX)
                    t += (pm as ModuleEnginesFX).maxThrust;
            }
            return t;
        }

        public static double CountMaxResourcesInChildren(this Part part, params DefaultResources[] resources) {
            var sum = CountMaxResources(part, resources);
            foreach(var p in part.children) {
                sum += p.CountMaxResourcesInChildren(resources);
            }
            return sum;
        }
        public static double CountRemainingResourcesInChildren(this Part part, params DefaultResources[] resources) {
            var sum = CountRemainingResources(part, resources);
            foreach(var p in part.children) {
                sum += p.CountRemainingResourcesInChildren(resources);
            }
            return sum;
        }
        public static bool CheckEnginesHaveFuelInChildren(this Part part) {
            if (part.CheckEngineHasFuel())
                return true;
            foreach (var c in part.children) {
                if (c.CheckEnginesHaveFuelInChildren())
                    return true;
            }
            return false;
        }
        public static float CountMaxThrustInChildren(this Part part) {
            var sum = part.GetMaxThrust();
            foreach(var p in part.children) {
                sum += p.CountMaxThrustInChildren();
            }
            return sum;
        }

        public static bool IsUnfiredDecoupler(this Part part) {
            foreach (PartModule m in part.Modules) {

                ModuleDecouple mDecouple = m as ModuleDecouple;
                if (mDecouple != null) {
                    if (!mDecouple.isDecoupled)
                        return true;
                    break;
                }
                ModuleAnchoredDecoupler mAnchoredDecoupler = m as ModuleAnchoredDecoupler;
                if (mAnchoredDecoupler != null) {
                    if (!mAnchoredDecoupler.isDecoupled)
                        return true;
                    break;
                }
            }
            return false;
        }
        public static bool IsStackSeperator(this Part part) {

            return part.ActivatesEvenIfDisconnected
                && !part.IsEngine()
                && part.IsDecoupledInStage(part.inverseStage)
                && part.isControlSource > 0;
        }
        public static bool IsSepratron(this Part part) {
            return part.ActivatesEvenIfDisconnected
                && part.IsEngine()
                && part.IsDecoupledInStage(part.inverseStage)
                && part.isControlSource > 0;
        }
        public static bool IsEngine(this Part part) {
            foreach (PartModule module in part.Modules) {
                if (module is ModuleEngines || module is ModuleEnginesFX)
                    return true;
            }
            return false;
        }
        public static bool IsDecoupledInStage(this Part part, int stage) {
            if ((part.IsUnfiredDecoupler() || part.IsLaunchClamp()) && part.inverseStage == stage)
                return true;
            if (part.parent == null)
                return false;
            return part.parent.IsDecoupledInStage(stage);
        }
        public static bool IsLaunchClamp(this Part part) {
            foreach (PartModule m in part.Modules) {
                if (m is LaunchClamp)
                    return true;
            }
            return false;
        }
    }
}
