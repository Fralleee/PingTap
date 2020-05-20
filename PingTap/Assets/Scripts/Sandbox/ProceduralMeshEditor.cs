#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProceduralMesh))]
public class ProceduralMeshEditor : Editor
{
  ProceduralMesh proceduralMesh;

  void OnEnable()
  {
    proceduralMesh = target as ProceduralMesh;
  }

  public override void OnInspectorGUI()
  {
    if (GUILayout.Button("Generate")) proceduralMesh.Generate();

    base.OnInspectorGUI();
  }
}
#endif