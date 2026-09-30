using System;
using System.Collections.Generic;

namespace HordeEvolution
{
    public readonly struct AbyssFloor
    {
        public readonly int floor;
        public readonly double hpMultiplier,damageMultiplier;
        public readonly string[] modifiers;
        public AbyssFloor(int f,double hp,double dmg,string[] mods){floor=f;hpMultiplier=hp;damageMultiplier=dmg;modifiers=mods;}
    }

    public static class AbyssGenerator
    {
        static readonly string[] Pool={
            "density_100","resurrect_once","chain_damage_minus_40","armored_shield",
            "elite_every_20","projectile_speed_80","enemy_speed_30"
        };
        public static AbyssFloor Generate(int floor)
        {
            var rng=new Random(floor*7919);
            var count=Math.Min(1+floor/25,4);
            var list=new List<string>();
            while(list.Count<count){var m=Pool[rng.Next(Pool.Length)];if(!list.Contains(m))list.Add(m);}
            return new AbyssFloor(floor,Math.Pow(1.075,floor-1),Math.Pow(1.035,floor-1),list.ToArray());
        }
    }
}
