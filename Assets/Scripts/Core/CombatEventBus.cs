using System;
using System.Collections.Generic;

namespace HordeEvolution
{
    public sealed class CombatEventBus
    {
        public const int MaxGeneration = 32;
        public const int MaxDescendantsPerRoot = 4096;
        public const int MaxEventsPerFrame = 1200;

        readonly Queue<CombatEvent> queue = new();
        readonly Dictionary<long,int> descendants = new();
        long nextId = 1;

        public event Action<CombatEvent> Resolved;

        public long NewRootId() => nextId++;

        public bool Enqueue(CombatEvent e)
        {
            if (e.generation > MaxGeneration) return false;
            if (e.rootEventId == 0) e.rootEventId = NewRootId();
            descendants.TryGetValue(e.rootEventId, out var count);
            if (count >= MaxDescendantsPerRoot) return false;
            e.eventId = nextId++;
            descendants[e.rootEventId] = count + 1;
            queue.Enqueue(e);
            return true;
        }

        public int ProcessFrame()
        {
            var n = Math.Min(queue.Count, MaxEventsPerFrame);
            for (var i=0;i<n;i++) {
                var e=queue.Dequeue();
                Resolved?.Invoke(e);
            }
            if(queue.Count==0) descendants.Clear();
            return n;
        }

        public void Clear(){ queue.Clear(); descendants.Clear(); }
    }
}
