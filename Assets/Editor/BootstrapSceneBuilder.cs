#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace HordeEvolution.Editor {
 public static class BootstrapSceneBuilder {
  [MenuItem("Horde Evolution/Create Prototype Scene")]
  public static void Build(){
   var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
   new GameObject("CombatRuntime").AddComponent<CombatRuntime>();
   new GameObject("Progression").AddComponent<ProgressionService>();
   var ground=GameObject.CreatePrimitive(PrimitiveType.Plane);ground.name="Arena";ground.transform.localScale=new Vector3(6,1,6);
   var player=GameObject.CreatePrimitive(PrimitiveType.Capsule);player.name="Player";
   Object.DestroyImmediate(player.GetComponent<CapsuleCollider>());player.AddComponent<CharacterController>();player.AddComponent<Health>();player.AddComponent<PlayerController>();player.AddComponent<AutoWeapon>();
   var cam=new GameObject("Main Camera");cam.tag="MainCamera";cam.AddComponent<Camera>();var follow=cam.AddComponent<TopDownCamera>();
   var so=new SerializedObject(follow);so.FindProperty("target").objectReferenceValue=player.transform;so.ApplyModifiedPropertiesWithoutUndo();
   EditorSceneManager.SaveScene(scene,"Assets/Prototype.unity");
   Debug.Log("Prototype created. Create Enemy prefab with Health + EnemyAgent, then add StageDirector and assign references.");
  }
 }
}
#endif
