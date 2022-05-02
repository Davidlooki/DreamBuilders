using UnityEngine;
using UnityEditor;
using System;

namespace DreamBuilders.Editor
{
    [CustomPropertyDrawer(typeof(LayerAttribute))]
    public class LayerPropertyDrawer : PropertyDrawerBase
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
                    DrawDefaultPropertyAndHelpBox(rect,
                                                  property,
                                                  string.Format(TypeWarningMessage, property.name));
                    break;
                }
            }

            EditorGUI.EndProperty();
        }

        private string[] GetLayers() =>
            UnityEditorInternal.InternalEditorUtility.layers;

        private static void DrawPropertyForString(Rect rect, SerializedProperty property, GUIContent label,
                                                  string[] layers)
        {
            int newIndex =
                EditorGUI.Popup(rect, label.text, IndexOf(layers, property.stringValue), layers);

            if (!property.stringValue.Equals(layers[newIndex], StringComparison.Ordinal))
                property.stringValue = layers[newIndex];
        }

        private static void DrawPropertyForInt(Rect rect, SerializedProperty property, GUIContent label,
                                               string[] layers)
        {
            int index = 0;
            string layerName = LayerMask.LayerToName(property.intValue);
            for (int i = 0; i < layers.Length; i++)
            {
                if (!layerName.Equals(layers[i], StringComparison.Ordinal)) continue;

                index = i;
                break;
            }

            int newLayerNumber =
                LayerMask.NameToLayer(layers[EditorGUI.Popup(rect, label.text, index, layers)]);

            if (property.intValue != newLayerNumber)
                property.intValue = newLayerNumber;
        }

        private static int IndexOf(string[] layers, string layer) =>
            Mathf.Clamp(Array.IndexOf(layers, layer), 0, layers.Length - 1);

        #endregion
    }
}