using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Fralle.Movement
{
  [CreateAssetMenu(menuName = "AI/WaypointSchema")]
  [Serializable]
  public class WaypointSchema : ScriptableObject
  {
    public List<Vector3> waypoints = new List<Vector3>();

    public void AddWaypoint()
    {
      waypoints.Add(waypoints.LastOrDefault());
    }

    public void RemoveWaypoint()
    {
      if (waypoints.Count > 0)
      {
        waypoints.RemoveAt(waypoints.Count - 1);
      }
    }

#if UNITY_EDITOR
    void OnEnable()
    {
      SceneView.duringSceneGui += OnSceneGUI;
    }

    void OnDisable()
    {
      SceneView.duringSceneGui -= OnSceneGUI;
    }

    void OnSceneGUI(SceneView sceneview)
    {
      if (Selection.activeObject != this) return;
      if (waypoints.Count == 0) return;

      Handles.DrawAAPolyLine(waypoints.ToArray());
      for (var i = 0; i < waypoints.Count; i++)
        waypoints[i] = Handles.PositionHandle(waypoints[i], Quaternion.identity);
    }
#endif
  }
}