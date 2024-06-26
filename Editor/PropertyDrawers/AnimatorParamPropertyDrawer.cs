using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace DreamBuilders.Editor
{
    [CustomPropertyDrawer(typeof(AnimatorParamAttribute))]
    public class AnimatorParamPropertyDrawer : PropertyDrawerBase
    {
        #region Fields

        private const string InvalidAnimatorControllerWarningMessage = "Target animator controller is null";
        private const string InvalidTypeWarningMessage = "{0} must be an int or a string";

        #endregion

        #region Constructors

        #endregion

        #region Methods

        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label) =>
            (property.propertyType is SerializedPropertyType.Integer or SerializedPropertyType.String
             && GetAnimatorController(property, PropertyUtility.GetAttribute<AnimatorParamAttribute>(property)
                                                               .AnimatorName) != null)
                ? GetPropertyHeight(property)
                : GetPropertyHeight(property) + GetHelpBoxHeight();

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            AnimatorParamAttribute animatorParamAttribute =
                PropertyUtility.GetAttribute<AnimatorParamAttribute>(property);

            AnimatorController animatorController =
                GetAnimatorController(property, animatorParamAttribute.AnimatorName);
            if (animatorController == null)
            {
                DrawDefaultPropertyAndHelpBox(rect, property, InvalidAnimatorControllerWarningMessage);
                return;
            }

            int parametersCount = animatorController.parameters.Length;
            List<AnimatorControllerParameter> animatorParameters = new(parametersCount);
            for (int i = 0; i < parametersCount; i++)
            {
                AnimatorControllerParameter parameter = animatorController.parameters[i];
                if (animatorParamAttribute.AnimatorParamType == null ||
                    parameter.type == animatorParamAttribute.AnimatorParamType)
                    animatorParameters.Add(parameter);
            }

            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                {
                    DrawPropertyForInt(rect, property, label, animatorParameters);
                    break;
                }
                case SerializedPropertyType.String:
                {
                    DrawPropertyForString(rect, property, label, animatorParameters);
                    break;
                }
                default:
                {
                    DrawDefaultPropertyAndHelpBox(rect, property,
                                                  string.Format(InvalidTypeWarningMessage, property.name));
                    break;
                }
            }

            EditorGUI.EndProperty();
        }

        private static void DrawPropertyForInt(Rect rect, SerializedProperty property, GUIContent label,
                                               IReadOnlyList<AnimatorControllerParameter> animatorParameters)
        {
            int paramNameHash = property.intValue;
            int index = 0;

            for (int i = 0; i < animatorParameters.Count; i++)
            {
                if (paramNameHash != animatorParameters[i].nameHash) continue;

                index = i + 1; // +1 because the first option is reserved for (None)
                break;
            }

            int newIndex = EditorGUI.Popup(rect, label.text, index, GetDisplayOptions(animatorParameters));
            int newValue = newIndex == 0 ? 0 : animatorParameters[newIndex - 1].nameHash;

            if (property.intValue != newValue)
                property.intValue = newValue;
        }

        private static void DrawPropertyForString(Rect rect, SerializedProperty property, GUIContent label,
                                                  IReadOnlyList<AnimatorControllerParameter> animatorParameters)
        {
            string paramName = property.stringValue;
            int index = 0;

            for (int i = 0; i < animatorParameters.Count; i++)
            {
                if (!paramName.Equals(animatorParameters[i].name, System.StringComparison.Ordinal)) continue;

                index = i + 1; // +1 because the first option is reserved for (None)
                break;
            }

            int newIndex = EditorGUI.Popup(rect, label.text, index, GetDisplayOptions(animatorParameters));
            string newValue = newIndex == 0 ? null : animatorParameters[newIndex - 1].name;

            if (!property.stringValue.Equals(newValue, System.StringComparison.Ordinal))
                property.stringValue = newValue;
        }

        private static string[] GetDisplayOptions(IReadOnlyList<AnimatorControllerParameter> animatorParams)
        {
            string[] displayOptions = new string[animatorParams.Count + 1];
            displayOptions[0] = "(None)";

            for (int i = 0; i < animatorParams.Count; i++)
                displayOptions[i + 1] = animatorParams[i].name;

            return displayOptions;
        }

        private static AnimatorController GetAnimatorController(SerializedProperty property, string animatorName)
        {
            object target = PropertyUtility.GetTargetObjectWithProperty(property);

            FieldInfo animatorFieldInfo = ReflectionUtility.GetField(target, animatorName);
            if (animatorFieldInfo != null &&
                animatorFieldInfo.FieldType == typeof(Animator))
            {
                Animator animator = animatorFieldInfo.GetValue(target) as Animator;
                if (animator != null)
                    return animator.runtimeAnimatorController as AnimatorController;
            }

            PropertyInfo animatorPropertyInfo = ReflectionUtility.GetProperty(target, animatorName);
            if (animatorPropertyInfo != null &&
                animatorPropertyInfo.PropertyType == typeof(Animator))
            {
                Animator animator = animatorPropertyInfo.GetValue(target) as Animator;
                if (animator != null)
                    return animator.runtimeAnimatorController as AnimatorController;
            }

            MethodInfo animatorGetterMethodInfo = ReflectionUtility.GetMethod(target, animatorName);
            if (animatorGetterMethodInfo == null || animatorGetterMethodInfo.ReturnType != typeof(Animator) ||
                animatorGetterMethodInfo.GetParameters().Length != 0) return null;
            {
                Animator animator = animatorGetterMethodInfo.Invoke(target, null) as Animator;
                if (animator == null) return null;

                return animator.runtimeAnimatorController as AnimatorController;
            }
        }

        #endregion
    }
}