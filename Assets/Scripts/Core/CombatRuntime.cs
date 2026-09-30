using System.Collections.Generic;
using UnityEngine;

namespace HordeEvolution
{
    public sealed class CombatRuntime : MonoBehaviour
    {
        public static CombatRuntime I { get; private set; }
        public readonly CombatEventBus Events = new();
        readonly Dictionary<int,IDamageable> entities = new();

        void Awake(){ I=this; Events.Resolved += Resolve; }
        void Update()=>Events.ProcessFrame();

        public void Register(IDamageable e)=>entities[e.EntityId]=e;
        public void Unregister(int id)=>entities.Remove(id);
        public bool TryGet(int id,out IDamageable e)=>entities.TryGetValue(id,out e);

        void Resolve(CombatEvent e)
        {
            if(e.type==CombatEventType.Damage && entities.TryGetValue(e.targetId,out var t) && t.IsAlive)
                t.TakeDamage(e.damage,e);
        }

        public void Damage(int source,int target,double damage,string weapon,string effect,long root=0,long parent=0,int generation=0)
        {
            Events.Enqueue(new CombatEvent {
                sourceId=source,targetId=target,damage=damage,weaponId=weapon,effectId=effect,
                type=CombatEventType.Damage,rootEventId=root,parentEventId=parent,generation=generation,
                timestamp=Time.timeAsDouble
            });
        }
    }
}
