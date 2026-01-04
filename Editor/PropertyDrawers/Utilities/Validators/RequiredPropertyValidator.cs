using UnityEditor;

namespace DreamBuilders.Editor
{
    public class RequiredPropertyValidator : PropertyValidatorBase
    {
        public override void ValidateProperty(SerializedProperty property)
        {
            RequiredAttribute requiredAttribute = PropertyUtility.GetAttribute<RequiredAttribute>(property);

            string message;
            MessageType messageType;

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue != null) return;

                message = !string.IsNullOrEmpty(requiredAttribute.Message)
                    ? requiredAttribute.Message
                    : property.name + " is required";

                messageType = MessageType.Error;
            }
            else
            {
                message = requiredAttribute.GetType().Name + " works only on reference types";
                messageType = MessageType.Warning;
            }

            DreamBuildersEditorGUI.HelpBox_Layout(message, messageType, property.serializedObject.targetObject);
        }
    }
}