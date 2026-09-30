using UnityEngine;

namespace HordeEvolution
{
    public sealed class StageDirector : MonoBehaviour
    {
        [SerializeField] Transform player;
        [SerializeField] EnemyAgent enemyPrefab;
        [SerializeField] int stageIndex=1;
        [SerializeField] int killTarget=40;
        [SerializeField] float spawnRadius=14;
        [SerializeField] int maxAlive=40;
        int kills,alive;
        public GamePhase Phase {get;private set;}=GamePhase.Combat;

        void Start()
        {
            var hp=player.GetComponent<Health>();
            if(hp)hp.Died += (_,__)=>Phase=GamePhase.Defeat;
        }

        void Update()
        {
            if(Phase!=GamePhase.Combat)return;
            while(alive<maxAlive && kills+alive<killTarget)Spawn();
            if(kills>=killTarget) {
                Phase=GamePhase.Victory;
                ProgressionService.I?.CompleteStage(stageIndex);
            }
        }

        void Spawn()
        {
            var a=Random.value*Mathf.PI*2;
            var p=player.position+new Vector3(Mathf.Cos(a),0,Mathf.Sin(a))*spawnRadius;
            var e=Instantiate(enemyPrefab,p,Quaternion.identity);
            var hp=100d*Mathf.Pow(1.085f,stageIndex-1);
            var dmg=8d*Mathf.Pow(1.055f,stageIndex-1);
            e.Init(player,hp,dmg,2.2f);
            alive++;
            e.Health.Died += OnEnemyDied;
        }

        void OnEnemyDied(Health h,CombatEvent evt)
        {
            kills++;alive--;
            ProgressionService.I?.AddCore(1);
            Destroy(h.gameObject);
        }
    }
}
