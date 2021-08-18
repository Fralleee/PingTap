#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Fralle.Gameplay
{
  [CustomEditor(typeof(Spawner)), CanEditMultipleObjects]
  public class SpawnerEditor : Editor
  {
    protected virtual void OnSceneGUI()
    {
      Spawner spawner = (Spawner)target;

      Handles.DrawWireDisc(spawner.SpawnCenter, Vector3.up, spawner.MinRange);
      Handles.DrawWireDisc(spawner.SpawnCenter, Vector3.up, spawner.MaxRange);

      EditorGUI.BeginChangeCheck();
      Vector3 newTargetPosition = Handles.PositionHandle(spawner.SpawnCenter, Quaternion.identity);
      if (EditorGUI.EndChangeCheck())
      {
        spawner.SpawnCenter = newTargetPosition;
      }
    }
  }
}
#endif
