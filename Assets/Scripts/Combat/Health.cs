using System;
using UnityEngine;

namespace HordeEvolution
{
    public sealed class Health : MonoBehaviour, IDamageable
    {
        static int ids=1;
        [SerializeField] double maxHp=100;
        public double Current { get; private set; }
        public int EntityId { get; private set; }
        public bool IsAlive => Current>0;
        public Vector3 Position => transform.position;
        public event Action<Health,CombatEvent> Died;
        public event Action<double,Vector3> Damaged;

        void Awake(){ EntityId=ids++; Current=maxHp; }
        void OnEnable(){ if(CombatRuntime.I) CombatRuntime.I.Register(this); }
        void Start(){ CombatRuntime.I?.Register(this); }
        void OnDisable(){ CombatRuntime.I?.Unregister(EntityId); }
        public void Configure(double hp){ maxHp=hp; Current=hp; }

        public void TakeDamage(double amount,CombatEvent e)
        {
            if(!IsAlive)return;
            amount=Math.Max(1,amount);
            Current=Math.Max(0,Current-amount);
            Damaged?.Invoke(amount,transform.position);
            if(Current<=0) Died?.Invoke(this,e);
        }
    }
}
