using UnityEditor;
using System.Reflection;
using DreamBuildersLibs;

namespace DreamBuilders.Editor
{
    public class ValidateInputPropertyValidator : PropertyValidatorBase
    {
        public override void ValidateProperty(SerializedProperty property)
        {
            ValidateInputAttribute validateInputAttribute = property.GetAttribute<ValidateInputAttribute>();

            object target = property.GetTargetObjectWithProperty();

            if (target.TryGetMethod(validateInputAttribute.CallbackName, out MethodInfo validationCallback)
                || validationCallback.ReturnType != typeof(bool))
                return;

            ParameterInfo[] callbackParameters = validationCallback.GetParameters();

            if (callbackParameters.Length == 0 && (bool)validationCallback.Invoke(target, null)) return;

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
                    target.TryGetField(property.name,out FieldInfo fieldInfo);

                    if (fieldInfo.FieldType == callbackParameters[0].ParameterType)
                    {
                        if ((bool)validationCallback.Invoke(target, new[] { fieldInfo.GetValue(target) })) return;

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
    }
}