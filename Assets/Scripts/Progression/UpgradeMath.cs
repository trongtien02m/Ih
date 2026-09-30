using System;

namespace HordeEvolution
{
    public static class UpgradeMath
    {
        public static long GlobalCost(long baseCost,int level)=>Clamp(baseCost*Math.Pow(1.16,level));
        public static long WeaponCost(long baseCost,int level)=>Clamp(baseCost*Math.Pow(1.18,level));
        static long Clamp(double v)=>v>=long.MaxValue?long.MaxValue:Math.Max(1,(long)Math.Ceiling(v));
    }
}
