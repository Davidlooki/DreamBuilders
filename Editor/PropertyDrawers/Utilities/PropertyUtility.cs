using UnityEditor;
using System.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DreamBuildersLibs;
using JetBrains.Annotations;
using UnityEngine;

namespace DreamBuilders.Editor
{
    public static class PropertyUtility
    {
        public static T GetAttribute<T>(this SerializedProperty property) where T : class
        {
            var attributes = GetAttributes<T>(property);

            return attributes.Length > 0 ? attributes[0] : null;
        }

        public static T[] GetAttributes<T>(this SerializedProperty property) where T : class =>
            !property.GetTargetObjectWithProperty().TryGetField(property.name, out FieldInfo fieldInfo)
                ? new T[] { }
                : (T[])fieldInfo.GetCustomAttributes(typeof(T), true);

        public static GUIContent GetLabel(this SerializedProperty property)
        {
            LabelAttribute labelAttribute = GetAttribute<LabelAttribute>(property);
            string labelText = (labelAttribute == null)
                ? property.displayName
                : labelAttribute.Label;

            GUIContent label = new(labelText);

            return label;
        }

        public static void CallOnValueChangedCallbacks(this SerializedProperty property)
        {
            OnValueChangedAttribute[] onValueChangedAttributes = GetAttributes<OnValueChangedAttribute>(property);

            if (onValueChangedAttributes.Length == 0)
                return;

            object target = GetTargetObjectWithProperty(property);
            property.serializedObject
                .ApplyModifiedProperties(); // We must apply modifications so that the new value is updated in the serialized object

            foreach (OnValueChangedAttribute onValueChangedAttribute in onValueChangedAttributes)
            {
                if (target.TryGetMethod(onValueChangedAttribute.CallbackName, out MethodInfo callbackMethod)
                    && callbackMethod.ReturnType == typeof(void)
                    && callbackMethod.GetParameters().Length == 0) { callbackMethod.Invoke(target, new object[] { }); }
                else
                {
                    string warning =
                        $"{onValueChangedAttribute.GetType().Name} can invoke only methods with 'void' return type and 0 parameters";

                    Debug.LogWarning(warning, property.serializedObject.targetObject);
                }
            }
        }

        public static bool IsEnabled(this SerializedProperty property)
        {
            ReadOnlyAttribute readOnlyAttribute = GetAttribute<ReadOnlyAttribute>(property);

            if (readOnlyAttribute != null)
                return false;

            EnableIfAttributeBase enableIfAttribute = GetAttribute<EnableIfAttributeBase>(property);

            if (enableIfAttribute == null)
                return true;

            object target = GetTargetObjectWithProperty(property);

            string message;

            // deal with enum conditions
            if (enableIfAttribute.EnumValue != null)
            {
                Enum value = GetEnumValue(target, enableIfAttribute.Conditions[0]);
                if (value != null)
                {
                    bool matched = value.GetType().GetCustomAttribute<FlagsAttribute>() == null
                        ? enableIfAttribute.EnumValue.Equals(value)
                        : value.HasFlag(enableIfAttribute.EnumValue);

                    return matched != enableIfAttribute.Inverted;
                }

                message = enableIfAttribute.GetType().Name +
                          " needs a valid enum field, property or method name to work";

                Debug.LogWarning(message, property.serializedObject.targetObject);

                return false;
            }

            // deal with normal conditions
            List<bool> conditionValues = GetConditionValues(target, enableIfAttribute.Conditions);
            if (conditionValues.Count > 0)
            {
                return GetConditionsFlag(conditionValues, enableIfAttribute.ConditionOperator,
                    enableIfAttribute.Inverted);
            }

            message = enableIfAttribute.GetType().Name +
                      " needs a valid boolean condition field, property or method name to work";

            Debug.LogWarning(message, property.serializedObject.targetObject);

            return false;
        }

        public static bool IsVisible(this SerializedProperty property)
        {
            ShowIfAttributeBase showIfAttribute = GetAttribute<ShowIfAttributeBase>(property);

            if (showIfAttribute == null)
                return true;

            object target = GetTargetObjectWithProperty(property);

            string message;

            // deal with enum conditions
            if (showIfAttribute.EnumValue != null)
            {
                Enum value = GetEnumValue(target, showIfAttribute.Conditions[0]);
                if (value != null)
                {
                    bool matched = value.GetType().GetCustomAttribute<FlagsAttribute>() == null
                        ? showIfAttribute.EnumValue.Equals(value)
                        : value.HasFlag(showIfAttribute.EnumValue);

                    return matched != showIfAttribute.Inverted;
                }

                message = showIfAttribute.GetType().Name +
                          " needs a valid enum field, property or method name to work";

                Debug.LogWarning(message, property.serializedObject.targetObject);

                return false;
            }

            // deal with normal conditions
            List<bool> conditionValues = GetConditionValues(target, showIfAttribute.Conditions);
            if (conditionValues.Count > 0)
            {
                return GetConditionsFlag(conditionValues, showIfAttribute.ConditionOperator,
                    showIfAttribute.Inverted);
            }

            message = showIfAttribute.GetType().Name +
                      " needs a valid boolean condition field, property or method name to work";

            Debug.LogWarning(message, property.serializedObject.targetObject);

            return false;
        }

        /// <summary>
        ///	Gets an enum value from reflection.
        /// </summary>
        /// <param name="target">The target object.</param>
        /// <param name="enumName">Name of a field, property, or method that returns an enum.</param>
        /// <returns>If it can't find an enum value, returns null.</returns>
        [CanBeNull]
        internal static Enum GetEnumValue(this object target, string enumName)
        {
            if (target.TryGetField(enumName, out FieldInfo enumField)
                && enumField.FieldType.IsSubclassOf(typeof(Enum)))
                return (Enum)enumField.GetValue(target);

            if (target.TryGetProperty(enumName, out PropertyInfo enumProperty)
                && enumProperty.PropertyType.IsSubclassOf(typeof(Enum)))
                return (Enum)enumProperty.GetValue(target);

            if (target.TryGetMethod(enumName, out MethodInfo enumMethod)
                && enumMethod.ReturnType.IsSubclassOf(typeof(Enum)))
                return (Enum)enumMethod.Invoke(target, null);

            return null;
        }

        internal static List<bool> GetConditionValues(this object target, string[] conditions)
        {
            List<bool> conditionValues = new();
            foreach (string condition in conditions)
            {
                if (target.TryGetField(condition, out FieldInfo conditionField)
                    && conditionField.FieldType == typeof(bool))
                    conditionValues.Add((bool)conditionField.GetValue(target));

                if (target.TryGetProperty(condition, out PropertyInfo conditionProperty)
                    && conditionProperty.PropertyType == typeof(bool))
                    conditionValues.Add((bool)conditionProperty.GetValue(target));

                if (target.TryGetMethod(condition, out MethodInfo conditionMethod)
                    && conditionMethod.ReturnType == typeof(bool) &&
                    conditionMethod.GetParameters().Length == 0)
                    conditionValues.Add((bool)conditionMethod.Invoke(target, null));
            }

            return conditionValues;
        }

        internal static bool GetConditionsFlag(
            this List<bool> conditionValues,
            EConditionOperator conditionOperator,
            bool invert
        )
        {
            bool flag = conditionOperator == EConditionOperator.And
                ? conditionValues.Aggregate(true, (current, value) => current && value)
                : conditionValues.Aggregate(false, (current, value) => current || value);

            if (invert)
                flag = !flag;

            return flag;
        }

        public static Type GetPropertyType(this SerializedProperty property) =>
            GetTargetObjectOfProperty(property).GetType();

        public static object GetTargetObjectOfProperty(this SerializedProperty property)
        {
            if (property == null) return null;

            string path = property.propertyPath.Replace(".Array.data[", "[");
            object obj = property.serializedObject.targetObject;
            string[] elements = path.Split('.');

            foreach (string element in elements)
            {
                if (element.Contains("["))
                {
                    string elementName = element.Substring(0, element.IndexOf("["));
                    int index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "")
                        .Replace("]", ""));

                    obj = GetValue_Imp(obj, elementName, index);
                }
                else
                    obj = GetValue_Imp(obj, element);
            }

            return obj;
        }

        public static object GetTargetObjectWithProperty(this SerializedProperty property)
        {
            string path = property.propertyPath.Replace(".Array.data[", "[");
            object obj = property.serializedObject.targetObject;
            string[] elements = path.Split('.');

            for (int i = 0; i < elements.Length - 1; i++)
            {
                string element = elements[i];
                if (element.Contains("["))
                {
                    string elementName = element.Substring(0, element.IndexOf("["));
                    int index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "")
                        .Replace("]", ""));

                    obj = GetValue_Imp(obj, elementName, index);
                }
                else
                    obj = GetValue_Imp(obj, element);
            }

            return obj;
        }

        private static object GetValue_Imp(this object source, string name)
        {
            if (source == null) return null;

            Type type = source.GetType();

            while (type != null)
            {
                FieldInfo field =
                    type.GetField(name,
                        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                if (field != null)
                    return field.GetValue(source);

                PropertyInfo property =
                    type.GetProperty(name,
                        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance |
                        BindingFlags.IgnoreCase);

                if (property != null)
                    return property.GetValue(source, null);

                type = type.BaseType;
            }

            return null;
        }

        private static object GetValue_Imp(this object source, string name, int index)
        {
            if (GetValue_Imp(source, name) is not IEnumerable enumerable) return null;

            IEnumerator enumerator = enumerable.GetEnumerator();

            for (int i = 0; i <= index; i++)
                if (!enumerator.MoveNext())
                    return null;

            return enumerator.Current;
        }
    }
}