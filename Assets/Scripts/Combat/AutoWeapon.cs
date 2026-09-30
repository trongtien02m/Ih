using System.Collections.Generic;
using UnityEngine;

namespace HordeEvolution
{
    public sealed class AutoWeapon : MonoBehaviour
    {
        public string weaponId="sword";
        [SerializeField] float range=3.2f;
        [SerializeField] float attacksPerSecond=1.2f;
        [SerializeField] double damage=15;
        [SerializeField] int cleave=3;
        [SerializeField] float skillRadius=5f;
        [SerializeField] double skillMultiplier=4;
        [SerializeField] float skillCooldown=10f;
        float nextAttack,nextSkill;
        Health owner;
        readonly List<EnemyAgent> targets=new();
        public float SkillCooldownRemaining=>Mathf.Max(0,nextSkill-Time.time);

        void Awake()=>owner=GetComponent<Health>();
        void Update()
        {
            if(Time.time<nextAttack)return;
            var target=TargetRegistry.Nearest(transform.position,range);
            if(!target)return;
            nextAttack=Time.time+1f/Mathf.Max(.05f,attacksPerSecond);
            Attack(target);
        }

        void Attack(EnemyAgent primary)
        {
            var root=CombatRuntime.I.Events.NewRootId();
            TargetRegistry.InRadius(primary.transform.position,1.8f,targets);
            var count=Mathf.Min(cleave,targets.Count);
            for(var i=0;i<count;i++)
                CombatRuntime.I.Damage(owner.EntityId,targets[i].Health.EntityId,damage,weaponId,"basic_slash",root);
        }

        public bool ActivateSkill()
        {
            if(Time.time<nextSkill)return false;
            nextSkill=Time.time+skillCooldown;
            var root=CombatRuntime.I.Events.NewRootId();
            TargetRegistry.InRadius(transform.position,skillRadius,targets);
            foreach(var e in targets)
                CombatRuntime.I.Damage(owner.EntityId,e.Health.EntityId,damage*skillMultiplier,weaponId,"active_skill",root);
            return true;
        }
    }
}
