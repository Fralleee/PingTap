#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace EPOOutline
{
	[CustomPropertyDrawer(typeof(Outlinable.OutlineProperties))]
	public class OutlinePropertiesPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Rect labelPosition = position;
			labelPosition.height = EditorGUIUtility.singleLineHeight;

			SerializedProperty enabledProperty = property.FindPropertyRelative("enabled");
			enabledProperty.boolValue = EditorGUI.ToggleLeft(labelPosition, label, enabledProperty.boolValue);

			Rect drawingPosition = position;
			drawingPosition.width -= EditorGUIUtility.singleLineHeight * 1.5f;
			drawingPosition.x += EditorGUIUtility.singleLineHeight;
			drawingPosition.height = EditorGUIUtility.singleLineHeight;

			NewLine(ref drawingPosition);

			EditorGUI.HelpBox(drawingPosition, "Dilate and blur setting will only work if 'Use info buffer' setting is set on Outliner", MessageType.Warning);

			NewLine(ref drawingPosition);

			Rect colorPosition = drawingPosition;
			colorPosition.width = 46;

			SerializedProperty colorProperty = property.FindPropertyRelative("color");
			colorProperty.colorValue = EditorGUI.ColorField(colorPosition, GUIContent.none, colorProperty.colorValue, true, true, true);

			float width = 45;
			Rect shiftPropertiesPositions = drawingPosition;
			shiftPropertiesPositions.x += colorPosition.width + EditorGUIUtility.standardVerticalSpacing;
			shiftPropertiesPositions.width -= colorPosition.width + EditorGUIUtility.standardVerticalSpacing;
			shiftPropertiesPositions.width /= 2;
			shiftPropertiesPositions.width -= EditorGUIUtility.standardVerticalSpacing + width;

			shiftPropertiesPositions.x += width;

			Rect labelRect = shiftPropertiesPositions;
			labelRect.x -= width - EditorGUIUtility.singleLineHeight * 0.25f;
			labelRect.width = width - EditorGUIUtility.singleLineHeight * 0.5f;

			EditorGUI.LabelField(labelRect, new GUIContent("Dilate"));

			EditorGUI.PropertyField(shiftPropertiesPositions, property.FindPropertyRelative("dilateShift"), GUIContent.none);

			shiftPropertiesPositions.x += shiftPropertiesPositions.width + EditorGUIUtility.standardVerticalSpacing + width;

			labelRect.x += shiftPropertiesPositions.width + width;
			EditorGUI.LabelField(labelRect, new GUIContent("Blur"));

			EditorGUI.PropertyField(shiftPropertiesPositions, property.FindPropertyRelative("blurShift"), GUIContent.none);

			NewLine(ref drawingPosition);

			EditorGUI.PropertyField(drawingPosition, property.FindPropertyRelative("fillPass"), new GUIContent("Fill parameters"));

			property.serializedObject.ApplyModifiedProperties();
		}

		private void NewLine(ref Rect rect)
		{
			rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 3.0f + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("fillPass"));
		}
	}
}
#endif
