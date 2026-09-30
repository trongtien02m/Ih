using System;
using System.Collections.Generic;
using UnityEngine;

namespace HordeEvolution
{
    [Serializable] public sealed class SaveData
    {
        public int saveVersion=1;
        public int highestCampaignStage=0, highestAbyssFloor=0;
        public long core=0;
        public string equippedWeapon="sword";
        public List<string> unlockedWeapons=new(){"sword"};
        public List<string> unlockedTraits=new();
        public List<string> equippedTraits=new();
        public List<string> unlockedEvolutions=new();
        public List<string> unlockedMutations=new();
    }

    public sealed class ProgressionService : MonoBehaviour
    {
        public static ProgressionService I {get;private set;}
        public SaveData Data {get;private set;}
        const string Key="horde_evolution_save_v1";
        void Awake(){I=this;DontDestroyOnLoad(gameObject);Load();}
        public void AddCore(long v){Data.core+=Math.Max(0,v);Save();}
        public void CompleteStage(int stage){Data.highestCampaignStage=Math.Max(Data.highestCampaignStage,stage);Save();}
        public void CompleteAbyss(int floor){Data.highestAbyssFloor=Math.Max(Data.highestAbyssFloor,floor);Save();}
        public void Unlock(string id,List<string> list){if(!list.Contains(id)){list.Add(id);Save();}}
        public void Save()=>PlayerPrefs.SetString(Key,JsonUtility.ToJson(Data));
        public void Load(){Data=PlayerPrefs.HasKey(Key)?JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(Key)):new SaveData();}
        void OnApplicationPause(bool p){if(p)Save();}
        void OnApplicationQuit()=>Save();
    }
}
