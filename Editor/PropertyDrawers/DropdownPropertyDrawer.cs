using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;
using System;
using System.Collections.Generic;
using DreamBuildersLibs;
using JetBrains.Annotations;

namespace DreamBuilders.Editor
{
    [CustomPropertyDrawer(typeof(DropdownAttribute))]
    public class DropdownPropertyDrawer : PropertyDrawerBase
    {
        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
        {
            DropdownAttribute dropdownAttribute = (DropdownAttribute)attribute;
            object values = GetValues(property, dropdownAttribute.ValuesName);
            property.GetTargetObjectWithProperty().TryGetField(property.name, out FieldInfo field);

            float propertyHeight = AreValuesValid(values, field)
                ? GetPropertyHeight(property)
                : GetPropertyHeight(property) + GetHelpBoxHeight();

            return propertyHeight;
        }

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            DropdownAttribute dropdownAttribute = (DropdownAttribute)attribute;
            object target = property.GetTargetObjectWithProperty();

            object valuesObject = GetValues(property, dropdownAttribute.ValuesName);
            target.TryGetField(property.name, out FieldInfo dropdownField);

            if (AreValuesValid(valuesObject, dropdownField))
            {
                switch (valuesObject)
                {
                    case IList valuesList when dropdownField.FieldType == GetElementType(valuesList):
                    {
                        // Selected value
                        object selectedValue = dropdownField.GetValue(target);

                        // Values and display options
                        object[] values = new object[valuesList.Count];
                        string[] displayOptions = new string[valuesList.Count];

                        for (int i = 0; i < values.Length; i++)
                        {
                            object value = valuesList[i];
                            values[i] = value;
                            displayOptions[i] = value == null ? "<null>" : value.ToString();
                        }

                        // Selected value index
                        int selectedValueIndex = Array.IndexOf(values, selectedValue);
                        if (selectedValueIndex < 0)
                            selectedValueIndex = 0;

                        DreamBuildersEditorGUI.Dropdown(
                            rect, property.serializedObject, target, dropdownField, label.text,
                            selectedValueIndex, values, displayOptions);

                        break;
                    }

                    case IDropdownList dropdown:
                    {
                        // Current value
                        object selectedValue = dropdownField.GetValue(target);

                        // Current value index, values and display options
                        int index = -1;
                        int selectedValueIndex = -1;
                        List<object> values = new();
                        List<string> displayOptions = new();

                        using IEnumerator<KeyValuePair<string, object>> dropdownEnumerator = dropdown.GetEnumerator();

                        while (dropdownEnumerator.MoveNext())
                        {
                            index++;

                            (string key, object value) = dropdownEnumerator.Current;

                            if (value?.Equals(selectedValue) == true)
                                selectedValueIndex = index;

                            values.Add(value);

                            displayOptions.Add(key == null
                                ? "<null>"
                                : string.IsNullOrWhiteSpace(key)
                                    ? "<empty>"
                                    : key);
                        }

                        if (selectedValueIndex < 0)
                            selectedValueIndex = 0;

                        DreamBuildersEditorGUI.Dropdown(
                            rect, property.serializedObject, target, dropdownField, label.text,
                            selectedValueIndex, values.ToArray(), displayOptions.ToArray());

                        break;
                    }
                }
            }
            else
            {
                string message =
                    $"Invalid values with name '{dropdownAttribute.ValuesName}' provided to '{dropdownAttribute.GetType().Name}'. Either the values name is incorrect or the types of the target field and the values field/property/method don't match";

                DrawDefaultPropertyAndHelpBox(rect, property, message);
            }

            EditorGUI.EndProperty();
        }

        [CanBeNull]
        private object GetValues(SerializedProperty property, string valuesName)
        {
            object target = property.GetTargetObjectWithProperty();

            return target.TryGetField(valuesName, out FieldInfo valuesFieldInfo)
                ? valuesFieldInfo.GetValue(target)
                : target.TryGetProperty(valuesName, out PropertyInfo valuesPropertyInfo)
                    ? valuesPropertyInfo.GetValue(target)
                    : target.TryGetMethod(valuesName, out MethodInfo methodValuesInfo)
                      && methodValuesInfo.ReturnType != typeof(void)
                      && methodValuesInfo.GetParameters().Length == 0
                        ? methodValuesInfo.Invoke(target, null)
                        : null;
        }

        private bool AreValuesValid(object values, FieldInfo dropdownField) =>
            values != null
            && dropdownField != null
            && ((values is IList
                 && dropdownField.FieldType == GetElementType(values))
                || values is IDropdownList);

        private Type GetElementType(object values) => values.GetType().GetListElementType();
    }
}