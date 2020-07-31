using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyManager)), CanEditMultipleObjects]
public class EnemyManagerEditor : Editor
{
  protected virtual void OnSceneGUI()
  {
    EnemyManager enemyManager = (EnemyManager)target;

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