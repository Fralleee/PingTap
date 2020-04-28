#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(WaveDefinition))]
public class WaveDefinitionInspector : PropertyDrawer
{
  public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label)
  {
    SerializedProperty sp = prop.FindPropertyRelative("enemy");
    SerializedProperty sp2 = prop.FindPropertyRelative("count");

    DrawEnemy(rect, sp);
    DrawCount(rect, sp2);
  }

  public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
  {
    return 48;
  }

  void DrawEnemy(Rect rect, SerializedProperty prop)
  {
    Object[] enemies = Resources.FindObjectsOfTypeAll<Enemy>();

    string[] options = enemies.Select(x => x.name).ToArray();
    int[] values = enemies.Select((x, i) => i).ToArray();

    int currentIndex = prop.objectReferenceValue ? Array.IndexOf(enemies, prop.objectReferenceValue) : 0;
    int selectValue = EditorGUI.IntPopup(new Rect(rect.x, rect.y, rect.width, 20), "Enemy", currentIndex,
      options, values);

    prop.objectReferenceValue = enemies[selectValue];
  }

  void DrawCount(Rect rect, SerializedProperty prop)
  {
    prop.intValue = EditorGUI.IntField(new Rect(rect.x, rect.y + 20, rect.width, 20), "Count", prop.intValue);
  }
}
#endif