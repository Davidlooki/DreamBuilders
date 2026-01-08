using UnityEditor;
using UnityEngine;

namespace DreamBuilders.Editor
{
    public abstract class PropertyDrawerBase : PropertyDrawer
    {
        public sealed override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
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

        protected abstract void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label);

        public sealed override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            !property.IsVisible() ? 0.0f : GetPropertyHeight_Internal(property, label);

        protected virtual float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label) =>
            EditorGUI.GetPropertyHeight(property, true);

        protected float GetPropertyHeight(SerializedProperty property)
        {
            SpecialCaseDrawerAttribute specialCaseAttribute =
                property.GetAttribute<SpecialCaseDrawerAttribute>();

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
    }
}