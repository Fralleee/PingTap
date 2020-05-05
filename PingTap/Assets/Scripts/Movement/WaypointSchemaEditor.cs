﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Fralle.Movement
{
  [CustomEditor(typeof(WaypointSchema))]
  public class WaypointSchemaEditor : Editor
  {
    WaypointSchema waypointSchema;

    void OnEnable()
    {
      waypointSchema = target as WaypointSchema;
    }

    public override void OnInspectorGUI()
    {
      GUILayout.BeginHorizontal();
      if (GUILayout.Button("Add waypoint")) waypointSchema.AddWaypoint();
      if (GUILayout.Button("Remove waypoint")) waypointSchema.RemoveWaypoint();
      GUILayout.EndHorizontal();

      base.OnInspectorGUI();
    }
  }
}
#endif