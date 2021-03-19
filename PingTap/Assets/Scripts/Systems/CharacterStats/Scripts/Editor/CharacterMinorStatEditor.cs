#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace CharacterStats
{
	[CustomPropertyDrawer(typeof(CharacterMinorStat))]
	public class CharacterMinorStatEditor : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 60f;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);


			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			var parentAttributeRect = new Rect(position.x, position.y, position.width - 32, 18);
			var parentStatFactorRect = new Rect(position.x + position.width - 30, position.y, 30, 18);
			var baseValueRect = new Rect(position.x, position.y + 20, 50, 18);
			var actualValueRect = new Rect(position.x + 52, position.y + 20, 50, 18);

			EditorGUI.PropertyField(parentAttributeRect, property.FindPropertyRelative("parentAttribute"), GUIContent.none);
			EditorGUI.PropertyField(parentStatFactorRect, property.FindPropertyRelative("parentStatFactor"), GUIContent.none);
			EditorGUI.PropertyField(baseValueRect, property.FindPropertyRelative("BaseValue"), GUIContent.none);
			EditorGUI.PropertyField(actualValueRect, property.FindPropertyRelative("_value"), GUIContent.none);

			EditorGUI.indentLevel = indent;

			EditorGUI.EndProperty();
		}
	}
}
#endif
