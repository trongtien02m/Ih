using System.Collections.Generic;
using UnityEngine;

namespace HordeEvolution
{
    public static class TargetRegistry
    {
        static readonly List<EnemyAgent> enemies=new();
        public static void Add(EnemyAgent e){ if(!enemies.Contains(e)) enemies.Add(e); }
        public static void Remove(EnemyAgent e)=>enemies.Remove(e);

        public static EnemyAgent Nearest(Vector3 p,float range)
        {
            EnemyAgent best=null; var d2=range*range;
            for(int i=enemies.Count-1;i>=0;i--) {
                var e=enemies[i];
                if(!e || !e.IsAlive){ enemies.RemoveAt(i); continue; }
                var d=(e.transform.position-p).sqrMagnitude;
                if(d<d2){d2=d;best=e;}
            }
            return best;
        }

        public static void InRadius(Vector3 p,float radius,List<EnemyAgent> result)
        {
            result.Clear(); var r2=radius*radius;
            foreach(var e in enemies) if(e && e.IsAlive && (e.transform.position-p).sqrMagnitude<=r2) result.Add(e);
        }
    }
}
