using UnityEditor;
using UnityEngine;

namespace DreamBuilders.Editor
{
    public abstract class SpecialCasePropertyDrawerBase
    {
        public void OnGUI(Rect rect, SerializedProperty property)
        {
            // Check if visible
            if (!property.IsVisible()) return;

            // Validate
            ValidatorAttribute[] validatorAttributes = property.GetAttributes<ValidatorAttribute>();
            foreach (ValidatorAttribute validatorAttribute in validatorAttributes)
                validatorAttribute.GetValidator().ValidateProperty(property);

            // Check if enabled and draw
            EditorGUI.BeginChangeCheck();
            using (new EditorGUI.DisabledScope(!property.IsEnabled()))
                OnGUI_Internal(rect, property, property.GetLabel());

            // Call OnValueChanged callbacks
            if (EditorGUI.EndChangeCheck())
                property.CallOnValueChangedCallbacks();
        }

        public float GetPropertyHeight(SerializedProperty property) =>
            GetPropertyHeight_Internal(property);

        protected abstract void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label);
        protected abstract float GetPropertyHeight_Internal(SerializedProperty property);
    }
}