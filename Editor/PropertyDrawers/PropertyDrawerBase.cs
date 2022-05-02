using UnityEditor;
using UnityEngine;

namespace DreamBuilders.Editor
{
    public abstract class PropertyDrawerBase : PropertyDrawer
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Methods

        public sealed override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
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

        protected abstract void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label);

        public sealed override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            !PropertyUtility.IsVisible(property) ? 0.0f : GetPropertyHeight_Internal(property, label);

        protected virtual float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label) =>
            EditorGUI.GetPropertyHeight(property, true);

        protected float GetPropertyHeight(SerializedProperty property)
        {
            SpecialCaseDrawerAttribute specialCaseAttribute =
                PropertyUtility.GetAttribute<SpecialCaseDrawerAttribute>(property);

            return specialCaseAttribute != null
                ? specialCaseAttribute.GetDrawer().GetPropertyHeight(property)
                : EditorGUI.GetPropertyHeight(property, true);
        }

        public float GetHelpBoxHeight() => EditorGUIUtility.singleLineHeight * 2.0f;

        public void DrawDefaultPropertyAndHelpBox(Rect rect, SerializedProperty property, string message)
        {
            float indentLength = DreamBuildersEditorGUI.GetIndentLength(rect);
            Rect helpBoxRect = new(rect.x + indentLength,
                                   rect.y,
                                   rect.width - indentLength,
                                   GetHelpBoxHeight());

            DreamBuildersEditorGUI.HelpBox(helpBoxRect, message, MessageType.Warning,
                                     property.serializedObject.targetObject);

            Rect propertyRect = new(rect.x,
                                    rect.y + GetHelpBoxHeight(),
                                    rect.width,
                                    GetPropertyHeight(property));

            EditorGUI.PropertyField(propertyRect, property, true);
        }

        #endregion
    }
}