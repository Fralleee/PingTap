using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Spawner)), CanEditMultipleObjects]
public class EnemyManagerEditor : Editor
{
  protected virtual void OnSceneGUI()
  {
    Spawner enemyManager = (Spawner)target;

    Handles.DrawWireDisc(enemyManager.spawnCenter, Vector3.up, enemyManager.minRange);
    Handles.DrawWireDisc(enemyManager.spawnCenter, Vector3.up, enemyManager.maxRange);

    EditorGUI.BeginChangeCheck();
    Vector3 newTargetPosition = Handles.PositionHandle(enemyManager.spawnCenter, Quaternion.identity);
    if (EditorGUI.EndChangeCheck())
    {
      enemyManager.spawnCenter = newTargetPosition;
    }
  }
}