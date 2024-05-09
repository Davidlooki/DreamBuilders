using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Text.RegularExpressions;
using System;

namespace DreamBuilders.Editor
{
    [CustomPropertyDrawer(typeof(SceneAttribute))]
    public class ScenePropertyDrawer : PropertyDrawerBase
    {
        private const string SceneListItem = "{0} ({1})";
        private const string ScenePattern = @".+\/(.+)\.unity";
        private const string TypeWarningMessage = "{0} must be an int or a string";
        private const string BuildSettingsWarningMessage = "No scenes in the build settings";

        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label) =>
            property.propertyType is SerializedPropertyType.String or SerializedPropertyType.Integer
            && GetScenes().Length > 0
                ? GetPropertyHeight(property)
                : GetPropertyHeight(property) + GetHelpBoxHeight();

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            string[] scenes = GetScenes();
            if (scenes.Length <= 0)
            {
                DrawDefaultPropertyAndHelpBox(rect, property, BuildSettingsWarningMessage);
                return;
            }

            string[] sceneOptions = GetSceneOptions(scenes);
            switch (property.propertyType)
            {
                case SerializedPropertyType.String:
                {
                    DrawPropertyForString(rect, property, label, scenes, sceneOptions);
                    break;
                }
                case SerializedPropertyType.Integer:
                {
                    DrawPropertyForInt(rect, property, label, sceneOptions);
                    break;
                }
                default:
                {
                    string message = string.Format(TypeWarningMessage, property.name);
                    DrawDefaultPropertyAndHelpBox(rect, property, message);
                    break;
                }
            }

            EditorGUI.EndProperty();
        }

        private string[] GetScenes() =>
            EditorBuildSettings.scenes
                               .Where(scene => scene.enabled)
                               .Select(scene => Regex.Match(scene.path, ScenePattern).Groups[1].Value)
                               .ToArray();

        private string[] GetSceneOptions(string[] scenes) =>
            scenes.Select((s, i) => string.Format(SceneListItem, s, i)).ToArray();

        private static void DrawPropertyForString(Rect rect, SerializedProperty property, GUIContent label,
                                                  string[] scenes, string[] sceneOptions)
        {
            int newIndex = EditorGUI.Popup(rect,
                                           label.text,
                                           IndexOf(scenes, property.stringValue),
                                           sceneOptions);

            if (!property.stringValue.Equals(scenes[newIndex], StringComparison.Ordinal))
                property.stringValue = scenes[newIndex];
        }

        private static void DrawPropertyForInt(Rect rect, SerializedProperty property, GUIContent label,
                                               string[] sceneOptions)
        {
            int newIndex = EditorGUI.Popup(rect, label.text, property.intValue, sceneOptions);

            if (property.intValue != newIndex)
                property.intValue = newIndex;
        }

        private static int IndexOf(string[] scenes, string scene) =>
            Mathf.Clamp(Array.IndexOf(scenes, scene), 0, scenes.Length - 1);
    }
}