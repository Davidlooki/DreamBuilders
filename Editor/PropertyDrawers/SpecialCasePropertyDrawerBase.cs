using UnityEditor;
using UnityEngine;

namespace DreamBuilders.Editor
{
    public abstract class SpecialCasePropertyDrawerBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Methods

        public void OnGUI(Rect rect, SerializedProperty property)
        {
            // Check if visible
            if (!PropertyUtility.IsVisible(property)) return;

            // Validate
            ValidatorAttribute[] validatorAttributes = PropertyUtility.GetAttributes<ValidatorAttribute>(property);
            foreach (ValidatorAttribute validatorAttribute in validatorAttributes)
                validatorAttribute.GetValidator().ValidateProperty(property);

            // Check if enabled and draw
            EditorGUI.BeginChangeCheck();
            using (new EditorGUI.DisabledScope(!PropertyUtility.IsEnabled(property)))
                OnGUI_Internal(rect, property, PropertyUtility.GetLabel(property));

            // Call OnValueChanged callbacks
            if (EditorGUI.EndChangeCheck())
                PropertyUtility.CallOnValueChangedCallbacks(property);
        }

        public float GetPropertyHeight(SerializedProperty property) =>
            GetPropertyHeight_Internal(property);

        protected abstract void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label);
        protected abstract float GetPropertyHeight_Internal(SerializedProperty property);

        #endregion
    }
}