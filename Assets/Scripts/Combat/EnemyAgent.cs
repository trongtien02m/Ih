using UnityEngine;

namespace HordeEvolution
{
    [RequireComponent(typeof(Health))]
    public sealed class EnemyAgent : MonoBehaviour
    {
        [SerializeField] float speed=2.4f;
        [SerializeField] float contactRange=1.2f;
        [SerializeField] double contactDamage=8;
        [SerializeField] float attackCooldown=1f;
        Transform player; Health health; float nextAttack;
        public bool IsAlive=>health && health.IsAlive;
        public Health Health=>health;

        void Awake(){health=GetComponent<Health>();}
        void OnEnable(){TargetRegistry.Add(this);}
        void OnDisable(){TargetRegistry.Remove(this);}
        public void Init(Transform target,double hp,double damage,float moveSpeed)
        { player=target; health.Configure(hp); contactDamage=damage; speed=moveSpeed; }

        void Update()
        {
            if(!player || !IsAlive)return;
            var delta=player.position-transform.position; delta.y=0;
            if(delta.sqrMagnitude>contactRange*contactRange) transform.position += delta.normalized*speed*Time.deltaTime;
            else if(Time.time>=nextAttack) {
                nextAttack=Time.time+attackCooldown;
                var ph=player.GetComponent<Health>();
                if(ph) CombatRuntime.I.Damage(health.EntityId,ph.EntityId,contactDamage,"enemy","contact");
            }
        }
    }
}
