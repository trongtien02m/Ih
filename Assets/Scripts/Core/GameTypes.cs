using System;
using UnityEngine;

namespace HordeEvolution
{
    public enum GamePhase { Build, Combat, Victory, Defeat }
    public enum ObjectiveType { Horde, EliteHunt, Boss, Assault }
    public enum CombatEventType { Attack, Hit, Crit, Damage, Kill, ProjectileSpawn, ProjectileHit, Slash, Chain, Split, Knockback, Collision, Explosion, Skill }
    [Flags] public enum ProcFlags { None=0, CannotCrit=1, NoOnKill=2 }

    [Serializable]
    public struct CombatEvent
    {
        public long eventId, parentEventId, rootEventId;
        public int sourceId, targetId;
        public string weaponId, effectId;
        public CombatEventType type;
        public double damage;
        public int generation;
        public ProcFlags flags;
        public double timestamp;
    }

    public interface IDamageable
    {
        int EntityId { get; }
        bool IsAlive { get; }
        Vector3 Position { get; }
        void TakeDamage(double amount, CombatEvent evt);
    }
}
