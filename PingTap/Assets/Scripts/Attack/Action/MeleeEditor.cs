#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Fralle.Attack.Action
{
  [CustomEditor(typeof(Melee))]
  public class MeleeEditor : Editor
  {
    void OnSceneGUI()
    {
      var t = target as Melee;

      Handles.color = new Color(1, 1, 1, 0.2f);
      Handles.DrawSolidArc(t.transform.position, t.transform.up, -t.transform.right, 180, t.meleeRadius);

      Handles.color = Color.white;
      t.meleeRadius = Handles.ScaleValueHandle(t.meleeRadius, t.transform.position + t.transform.forward * t.meleeRadius, t.transform.rotation, 1, Handles.ConeHandleCap, 1);
    }
  }
}
#endif