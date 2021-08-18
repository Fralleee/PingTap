#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EPOOutline
{
  [CustomPropertyDrawer(typeof(SerializedPass))]
  public class SerializedPassPropertyDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      Rect drawingPosition = position;
      drawingPosition.height = EditorGUIUtility.singleLineHeight;

      SerializedProperty shaderProperty = property.FindPropertyRelative("shader");

      Shader currentShaderReference = shaderProperty.objectReferenceValue as Shader;
      string prefix = "Hidden/EPO/Fill/";
      string fillLabel = currentShaderReference == null ? "none" : currentShaderReference.name.Substring(prefix.Length);
      if (shaderProperty.hasMultipleDifferentValues)
        fillLabel = "-";

      if (EditorGUI.DropdownButton(position, new GUIContent("Fill type: " + fillLabel), FocusType.Passive))
      {
        GenericMenu menu = new GenericMenu();

        menu.AddItem(new GUIContent("none"), currentShaderReference == null && !shaderProperty.hasMultipleDifferentValues, () =>
            {
              shaderProperty.objectReferenceValue = null;
              shaderProperty.serializedObject.ApplyModifiedProperties();
            });

        string[] shaders = AssetDatabase.FindAssets("t:Shader");
        foreach (string shader in shaders)
        {
          Shader loadedShader = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(shader), typeof(Shader)) as Shader;
          if (!loadedShader.name.StartsWith(prefix))
            continue;

          menu.AddItem(new GUIContent(loadedShader.name.Substring(prefix.Length)), loadedShader == shaderProperty.objectReferenceValue && !shaderProperty.hasMultipleDifferentValues, () =>
              {
                shaderProperty.objectReferenceValue = loadedShader;
                shaderProperty.serializedObject.ApplyModifiedProperties();
              });
        }

        menu.ShowAsContext();
      }

      if (shaderProperty.hasMultipleDifferentValues)
        return;

      if (currentShaderReference != null)
      {
        position.x += EditorGUIUtility.singleLineHeight;
        position.width -= EditorGUIUtility.singleLineHeight;
        Dictionary<string, SerializedProperty> properties = new Dictionary<string, SerializedProperty>();

        SerializedProperty serializedProperties = property.FindPropertyRelative("serializedProperties");

        for (int index = 0; index < serializedProperties.arraySize; index++)
        {
          SerializedProperty subProperty = serializedProperties.GetArrayElementAtIndex(index);

          SerializedProperty propertyName = subProperty.FindPropertyRelative("PropertyName");
          SerializedProperty propertyValue = subProperty.FindPropertyRelative("Property");

          if (propertyName == null || propertyValue == null)
            break;

          properties.Add(propertyName.stringValue, propertyValue);
        }

        Rect fillParametersPosition = position;
        for (int index = 0; index < ShaderUtil.GetPropertyCount(currentShaderReference); index++)
        {
          string propertyName = ShaderUtil.GetPropertyName(currentShaderReference, index);
          if (!propertyName.StartsWith("_Public"))
            continue;

          fillParametersPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

          SerializedProperty currentProperty;
          if (!properties.TryGetValue(propertyName, out currentProperty))
          {
            serializedProperties.InsertArrayElementAtIndex(serializedProperties.arraySize);
            currentProperty = serializedProperties.GetArrayElementAtIndex(serializedProperties.arraySize - 1);
            currentProperty.FindPropertyRelative("PropertyName").stringValue = propertyName;
            currentProperty = currentProperty.FindPropertyRelative("Property");

            Material tempMaterial = new Material(currentShaderReference);

            switch (ShaderUtil.GetPropertyType(currentShaderReference, index))
            {
              case ShaderUtil.ShaderPropertyType.Color:
                currentProperty.FindPropertyRelative("ColorValue").colorValue = tempMaterial.GetColor(propertyName);
                break;
              case ShaderUtil.ShaderPropertyType.Vector:
                currentProperty.FindPropertyRelative("VectorValue").vector4Value = tempMaterial.GetVector(propertyName);
                break;
              case ShaderUtil.ShaderPropertyType.Float:
                currentProperty.FindPropertyRelative("FloatValue").floatValue = tempMaterial.GetFloat(propertyName);
                break;
              case ShaderUtil.ShaderPropertyType.Range:
                currentProperty.FindPropertyRelative("FloatValue").floatValue = tempMaterial.GetFloat(propertyName);
                break;
              case ShaderUtil.ShaderPropertyType.TexEnv:
                currentProperty.FindPropertyRelative("TextureValue").objectReferenceValue = tempMaterial.GetTexture(propertyName);
                break;
            }

            Object.DestroyImmediate(tempMaterial);

            properties.Add(propertyName, currentProperty);
          }

          if (currentProperty == null)
            continue;

          GUIContent content = new GUIContent(ShaderUtil.GetPropertyDescription(currentShaderReference, index));

          switch (ShaderUtil.GetPropertyType(currentShaderReference, index))
          {
            case ShaderUtil.ShaderPropertyType.Color:
              SerializedProperty colorProperty = currentProperty.FindPropertyRelative("ColorValue");
              colorProperty.colorValue = EditorGUI.ColorField(fillParametersPosition, content, colorProperty.colorValue, true, true, true);
              break;
            case ShaderUtil.ShaderPropertyType.Vector:
              SerializedProperty vectorProperty = currentProperty.FindPropertyRelative("VectorValue");
              vectorProperty.vector4Value = EditorGUI.Vector4Field(fillParametersPosition, content, vectorProperty.vector4Value);
              break;
            case ShaderUtil.ShaderPropertyType.Float:
              EditorGUI.PropertyField(fillParametersPosition, currentProperty.FindPropertyRelative("FloatValue"), content);
              break;
            case ShaderUtil.ShaderPropertyType.Range:
              SerializedProperty floatProperty = currentProperty.FindPropertyRelative("FloatValue");
              floatProperty.floatValue = EditorGUI.Slider(fillParametersPosition, content, floatProperty.floatValue,
                  ShaderUtil.GetRangeLimits(currentShaderReference, index, 1),
                  ShaderUtil.GetRangeLimits(currentShaderReference, index, 2));
              break;
            case ShaderUtil.ShaderPropertyType.TexEnv:
              EditorGUI.PropertyField(fillParametersPosition, currentProperty.FindPropertyRelative("TextureValue"), content);
              break;
          }

          currentProperty.FindPropertyRelative("PropertyType").intValue = (int)ShaderUtil.GetPropertyType(currentShaderReference, index);
        }
      }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      if (property.FindPropertyRelative("shader").hasMultipleDifferentValues)
        return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

      SerializedProperty shaderProperty = property.FindPropertyRelative("shader");
      Shader currentShaderReference = shaderProperty.objectReferenceValue as Shader;

      int additionalCount = 0;
      if (currentShaderReference != null)
      {
        for (int index = 0; index < ShaderUtil.GetPropertyCount(currentShaderReference); index++)
        {
          string propertyName = ShaderUtil.GetPropertyName(currentShaderReference, index);
          if (!propertyName.StartsWith("_Public"))
            continue;

          additionalCount++;
        }
      }

      return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * (additionalCount + 1);
    }
  }
}
#endif
