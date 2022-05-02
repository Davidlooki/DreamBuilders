using UnityEditor;
using System.Reflection;
using System;

namespace DreamBuilders.Editor
{
    public class ValidateInputPropertyValidator : PropertyValidatorBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Methods

        public override void ValidateProperty(SerializedProperty property)
        {
            ValidateInputAttribute validateInputAttribute =
                PropertyUtility.GetAttribute<ValidateInputAttribute>(property);
            object target = PropertyUtility.GetTargetObjectWithProperty(property);

            MethodInfo validationCallback = ReflectionUtility.GetMethod(target, validateInputAttribute.CallbackName);

            if (validationCallback == null || validationCallback.ReturnType != typeof(bool)) return;

            ParameterInfo[] callbackParameters = validationCallback.GetParameters();

            if (callbackParameters.Length == 0 && (bool) validationCallback.Invoke(target, null)) return;

            switch (callbackParameters.Length)
            {
                case 0:
                    DreamBuildersEditorGUI.HelpBox_Layout(string.IsNullOrEmpty(validateInputAttribute.Message)
                                                        ? property.name + " is not valid"
                                                        : validateInputAttribute.Message,
                                                    MessageType.Error,
                                                    property.serializedObject.targetObject);
                    break;
                case 1:
                {
                    FieldInfo fieldInfo = ReflectionUtility.GetField(target, property.name);

                    if (fieldInfo.FieldType == callbackParameters[0].ParameterType)
                    {
                        if ((bool) validationCallback.Invoke(target, new[] {fieldInfo.GetValue(target)})) return;

                        DreamBuildersEditorGUI.HelpBox_Layout(string.IsNullOrEmpty(validateInputAttribute.Message)
                                                            ? property.name + " is not valid"
                                                            : validateInputAttribute.Message,
                                                        MessageType.Error,
                                                        property.serializedObject.targetObject);
                    }
                    else
                    {
                        const string warning = "The field type is not the same as the callback's parameter type";
                        DreamBuildersEditorGUI.HelpBox_Layout(warning,
                                                        MessageType.Warning,
                                                        property.serializedObject.targetObject);
                    }

                    break;
                }
                default:
                {
                    string warning = validateInputAttribute.GetType().Name +
                                     " needs a callback with boolean return type and an optional single parameter of the same type as the field";

                    DreamBuildersEditorGUI.HelpBox_Layout(warning,
                                                    MessageType.Warning,
                                                    property.serializedObject.targetObject);
                    break;
                }
            }
        }

        #endregion
    }
}