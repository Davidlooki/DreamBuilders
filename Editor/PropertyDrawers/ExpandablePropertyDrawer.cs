using System;
using UnityEditor;
using UnityEngine;

namespace DreamBuilders.Editor
{
    [CustomPropertyDrawer(typeof(ExpandableAttribute))]
    public class ExpandablePropertyDrawer : PropertyDrawerBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Methods

        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
        {
            if (property.objectReferenceValue == null)
                return GetPropertyHeight(property);

            if (!typeof(ScriptableObject).IsAssignableFrom(PropertyUtility.GetPropertyType(property)))
                return GetPropertyHeight(property) + GetHelpBoxHeight();

            ScriptableObject scriptableObject = property.objectReferenceValue as ScriptableObject;

            if (scriptableObject == null)
                return GetPropertyHeight(property);

            if (!property.isExpanded)
                return GetPropertyHeight(property);

            using SerializedObject serializedObject = new(scriptableObject);
            float totalHeight = EditorGUIUtility.singleLineHeight;

            using SerializedProperty iterator = serializedObject.GetIterator();

            if (iterator.NextVisible(true))
            {
                do
                {
                    SerializedProperty childProperty = serializedObject.FindProperty(iterator.name);
                    if (childProperty.name.Equals("m_Script", StringComparison.Ordinal))
                        continue;

                    if (!PropertyUtility.IsVisible(childProperty))
                        continue;

                    totalHeight += GetPropertyHeight(childProperty);
                } while (iterator.NextVisible(false));
            }

            totalHeight += EditorGUIUtility.standardVerticalSpacing;
            return totalHeight;
        }

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            if (property.objectReferenceValue == null)
            {
                EditorGUI.PropertyField(rect, property, label, false);
            }
            else
            {
                if (typeof(ScriptableObject).IsAssignableFrom(PropertyUtility.GetPropertyType(property)))
                {
                    if ((ScriptableObject)property.objectReferenceValue == null)
                    {
                        EditorGUI.PropertyField(rect, property, label, false);
                    }
                    else
                    {
                        // Draw a foldout
                        Rect foldoutRect = new()
                        {
                            x = rect.x,
                            y = rect.y,
                            width = EditorGUIUtility.labelWidth,
                            height = EditorGUIUtility.singleLineHeight
                        };

                        property.isExpanded =
                            EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

                        // Draw the scriptable object field
                        Rect propertyRect = new()
                        {
                            x = rect.x,
                            y = rect.y,
                            width = rect.width,
                            height = EditorGUIUtility.singleLineHeight
                        };

                        EditorGUI.PropertyField(propertyRect, property, label, false);

                        // Draw the child properties
                        if (property.isExpanded)
                            DrawChildProperties(rect, property);
                    }
                }
                else
                {
                    //string message = $"{typeof(ExpandableAttribute).Name} can only be used on scriptable objects";
                    string message = $"{nameof(ExpandableAttribute)} can only be used on scriptable objects";
                    DrawDefaultPropertyAndHelpBox(rect, property, message);
                }
            }

            property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
        }

        private void DrawChildProperties(Rect rect, SerializedProperty property)
        {
            ScriptableObject scriptableObject = property.objectReferenceValue as ScriptableObject;
            if (scriptableObject == null)
                return;

            Rect boxRect = new()
            {
                x = 0.0f,
                y = rect.y + EditorGUIUtility.singleLineHeight,
                width = rect.width * 2.0f,
                height = rect.height - EditorGUIUtility.singleLineHeight
            };

            GUI.Box(boxRect, GUIContent.none);

            using (new EditorGUI.IndentLevelScope())
            {
                SerializedObject serializedObject = new(scriptableObject);
                serializedObject.Update();

                using SerializedProperty iterator = serializedObject.GetIterator();

                float yOffset = EditorGUIUtility.singleLineHeight;

                if (iterator.NextVisible(true))
                {
                    do
                    {
                        SerializedProperty childProperty = serializedObject.FindProperty(iterator.name);
                        if (childProperty.name.Equals("m_Script", StringComparison.Ordinal))
                            continue;

                        if (!PropertyUtility.IsVisible(childProperty))
                            continue;

                        float childHeight = GetPropertyHeight(childProperty);
                        Rect childRect = new()
                        {
                            x = rect.x,
                            y = rect.y + yOffset,
                            width = rect.width,
                            height = childHeight
                        };

                        DreamBuildersEditorGUI.PropertyField(childRect, childProperty, true);

                        yOffset += childHeight;
                    } while (iterator.NextVisible(false));
                }

                serializedObject.ApplyModifiedProperties();
            }
        }

        #endregion
    }
}