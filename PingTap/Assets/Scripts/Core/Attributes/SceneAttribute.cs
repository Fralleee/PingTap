using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Fralle.Core.Attributes
{
  public class SceneAttribute : PropertyAttribute
  {
  }


#if UNITY_EDITOR
  [CustomPropertyDrawer(typeof(SceneAttribute))]
  public class SceneDrawer : PropertyDrawer
  {
    const string SceneListItem = "{0} ({1})";
    const string ScenePattern = @".+\/(.+)\.unity";

    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
      var scenes = GetScenes();
      var sceneOptions = GetSceneOptions(scenes);
      switch (property.propertyType)
      {
        case SerializedPropertyType.String:
          DrawPropertyForString(rect, property, label, scenes, sceneOptions);
          break;
        case SerializedPropertyType.Integer:
          DrawPropertyForInt(rect, property, label, sceneOptions);
          break;
      }
    }

    string[] GetScenes()
    {
      return EditorBuildSettings.scenes
        .Where(scene => scene.enabled)
        .Select(scene => Regex.Match(scene.path, ScenePattern).Groups[1].Value)
        .ToArray();
    }

    string[] GetSceneOptions(string[] scenes)
    {
      return scenes.Select((s, i) => string.Format(SceneListItem, s, i)).ToArray();
    }

    static void DrawPropertyForString(Rect rect, SerializedProperty property, GUIContent label, string[] scenes, string[] sceneOptions)
    {
      var index = IndexOf(scenes, property.stringValue);
      var newIndex = EditorGUI.Popup(rect, label.text, index, sceneOptions);
      property.stringValue = scenes[newIndex];
    }

    static void DrawPropertyForInt(Rect rect, SerializedProperty property, GUIContent label, string[] sceneOptions)
    {
      var index = property.intValue;
      var newIndex = EditorGUI.Popup(rect, label.text, index, sceneOptions);
      property.intValue = newIndex;
    }

    static int IndexOf(string[] scenes, string scene)
    {
      var index = Array.IndexOf(scenes, scene);
      return Mathf.Clamp(index, 0, scenes.Length - 1);
    }
  }
#endif

}