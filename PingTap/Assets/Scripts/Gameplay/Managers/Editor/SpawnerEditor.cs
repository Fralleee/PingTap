using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Spawner)), CanEditMultipleObjects]
public class SpawnerEditor : Editor
{
  protected virtual void OnSceneGUI()
  {
    Spawner spawner = (Spawner)target;

    Handles.DrawWireDisc(spawner.spawnCenter, Vector3.up, spawner.minRange);
    Handles.DrawWireDisc(spawner.spawnCenter, Vector3.up, spawner.maxRange);

    EditorGUI.BeginChangeCheck();
    Vector3 newTargetPosition = Handles.PositionHandle(spawner.spawnCenter, Quaternion.identity);
    if (EditorGUI.EndChangeCheck())
    {
      spawner.spawnCenter = newTargetPosition;
    }
  }
}