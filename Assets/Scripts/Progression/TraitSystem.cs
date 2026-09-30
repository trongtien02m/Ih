using System;
using System.Collections.Generic;

namespace HordeEvolution
{
    public enum TriggerType { Hit, Crit, Kill, Skill, Explosion }

    [Serializable] public sealed class TraitDefinition
    {
        public string id;
        public TriggerType trigger;
        public string emittedEffect;
        public float chance=1f;
        public double multiplier=1;
    }

    public sealed class TraitRuntime
    {
        readonly List<TraitDefinition> traits=new();
        readonly Random rng=new();
        public void Set(IEnumerable<TraitDefinition> source){traits.Clear();traits.AddRange(source);}
        public IEnumerable<TraitDefinition> Match(TriggerType t)
        {
            foreach(var x in traits) if(x.trigger==t && rng.NextDouble()<=x.chance) yield return x;
        }
    }
}
