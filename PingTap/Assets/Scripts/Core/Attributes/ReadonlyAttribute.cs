using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Fralle.Core.Attributes
{
  public class ReadonlyAttribute : PropertyAttribute
  {

  }

#if UNITY_EDITOR
  [CustomPropertyDrawer(typeof(ReadonlyAttribute))]
  public class ReadonlyDrawer : PropertyDrawer
  {
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      GUI.enabled = false;
      EditorGUI.PropertyField(position, property, label, true);
      GUI.enabled = true;
    }
  }
#endif
}