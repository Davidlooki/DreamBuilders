using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DreamBuilders.AbilitySystem.Editor
{
    [CustomPropertyDrawer(typeof(IAbilityEffect))]
    public class AbilityEffectDrawer : PropertyDrawer
    {
        private static Dictionary<string, Type> _typeMap;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_typeMap == null) BuildTypeMap();

            var typeRect = new Rect(
                position.x,
                position.y,
                position.width,
                EditorGUIUtility.singleLineHeight);

            var contentRect = new Rect(
                position.x,
                position.y + EditorGUIUtility.singleLineHeight,
                position.width,
                position.height - EditorGUIUtility.singleLineHeight);

            EditorGUI.BeginProperty(position, label, property);
            var typeName = property.managedReferenceFullTypename;
            var displayName = GetShortTypeName(typeName);

            if (EditorGUI.DropdownButton(
                    typeRect,
                    new GUIContent(displayName ?? "Select effect type"),
                    FocusType.Keyboard))
            {
                var menu = new GenericMenu();
                if (_typeMap == null || _typeMap.Count == 0)
                {
                    menu.AddDisabledItem(new GUIContent("No AbilityEffect available"));
                    menu.ShowAsContext();

                    return;
                }

                foreach (var kvp in _typeMap)
                {
                    var name = kvp.Key;
                    var type = kvp.Value;
                    menu.AddItem(new GUIContent(name), type.FullName == typeName, () =>
                    {
                        property.managedReferenceValue = Activator.CreateInstance(type);
                        property.serializedObject.ApplyModifiedProperties();
                    });
                }

                menu.ShowAsContext();
            }

            if (property.managedReferenceValue != null)
            {
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(contentRect, property, GUIContent.none, true);
                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            EditorGUI.GetPropertyHeight(property, label, true) + EditorGUIUtility.singleLineHeight;

        private void BuildTypeMap()
        {
            var baseType = typeof(IAbilityEffect);
            _typeMap = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(asm =>
                {
                    try { return asm.GetTypes(); }
                    catch { return Type.EmptyTypes; }
                })
                .Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t))
                .ToDictionary(t => ObjectNames.NicifyVariableName(t.Name), t => t);
        }

        private static string GetShortTypeName(string fullTypeName)
        {
            if (string.IsNullOrEmpty(fullTypeName)) return null;
            var parts = fullTypeName.Split(' ');

            return parts.Length >= 1 ? parts[1].Split('.').Last() : fullTypeName;
        }
    }
}