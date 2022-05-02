using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace DreamBuilders.Editor
{
    [CustomPropertyDrawer(typeof(SortingLayerAttribute))]
    public class SortingLayerPropertyDrawer : PropertyDrawerBase
    {
        #region Fields

        private const string TypeWarningMessage = "{0} must be an int or a string";

        #endregion

        #region Constructors

        #endregion

        #region Methods

        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label) =>
            property.propertyType
                is SerializedPropertyType.String
                   or SerializedPropertyType.Integer
                ? GetPropertyHeight(property)
                : GetPropertyHeight(property) + GetHelpBoxHeight();

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            switch (property.propertyType)
            {
                case SerializedPropertyType.String:
                {
                    DrawPropertyForString(rect, property, label, GetLayers());
                    break;
                }
                case SerializedPropertyType.Integer:
                {
                    DrawPropertyForInt(rect, property, label, GetLayers());
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

        private string[] GetLayers()
        {
            Type internalEditorUtilityType = typeof(UnityEditorInternal.InternalEditorUtility);
            PropertyInfo sortingLayersProperty =
                internalEditorUtilityType.GetProperty("sortingLayerNames",
                                                      BindingFlags.Static | BindingFlags.NonPublic);
            return (string[]) sortingLayersProperty?.GetValue(null, Array.Empty<object>());
        }

        private static void DrawPropertyForString(Rect rect, SerializedProperty property, GUIContent label,
                                                  string[] layers)
        {
            int index = IndexOf(layers, property.stringValue);
            int newIndex = EditorGUI.Popup(rect, label.text, index, layers);
            string newLayer = layers[newIndex];

            if (!property.stringValue.Equals(newLayer, StringComparison.Ordinal))
                property.stringValue = layers[newIndex];
        }

        private static void DrawPropertyForInt(Rect rect, SerializedProperty property, GUIContent label,
                                               string[] layers)
        {
            int index = 0;
            string layerName = SortingLayer.IDToName(property.intValue);
            for (int i = 0; i < layers.Length; i++)
            {
                if (!layerName.Equals(layers[i], StringComparison.Ordinal)) continue;

                index = i;
                break;
            }

            int newLayerNumber = SortingLayer.NameToID(layers[EditorGUI.Popup(rect, label.text, index, layers)]);

            if (property.intValue != newLayerNumber)
                property.intValue = newLayerNumber;
        }

        private static int IndexOf(string[] layers, string layer) =>
            Mathf.Clamp(Array.IndexOf(layers, layer), 0, layers.Length - 1);

        #endregion
    }
}