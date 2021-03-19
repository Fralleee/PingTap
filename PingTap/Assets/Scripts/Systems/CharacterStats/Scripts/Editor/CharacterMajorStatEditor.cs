#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace CharacterStats
{
	[CustomPropertyDrawer(typeof(CharacterMajorStat))]
	public class CharacterMajorStatEditor : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			var baseValueRect = new Rect(position.x, position.y, position.width, 18);

			EditorGUI.PropertyField(baseValueRect, property.FindPropertyRelative("BaseValue"), GUIContent.none);
			EditorGUI.indentLevel = indent;

			EditorGUI.EndProperty();
		}
	}
}
#endif
