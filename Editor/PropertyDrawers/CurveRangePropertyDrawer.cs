using UnityEngine;
using UnityEditor;

namespace DreamBuilders.Editor
{
    [CustomPropertyDrawer(typeof(CurveRangeAttribute))]
    public class CurveRangePropertyDrawer : PropertyDrawerBase
    {
        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label) =>
            property.propertyType == SerializedPropertyType.AnimationCurve
                ? GetPropertyHeight(property)
                : GetPropertyHeight(property) + GetHelpBoxHeight();


        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            // Check user error
            if (property.propertyType != SerializedPropertyType.AnimationCurve)
            {
                string message = $"Field {property.name} is not an AnimationCurve";
                DrawDefaultPropertyAndHelpBox(rect, property, message);
                return;
            }

            CurveRangeAttribute curveRangeAttribute = (CurveRangeAttribute) attribute;
            Rect curveRanges = new(
                                   curveRangeAttribute.Min.x,
                                   curveRangeAttribute.Min.y,
                                   curveRangeAttribute.Max.x - curveRangeAttribute.Min.x,
                                   curveRangeAttribute.Max.y - curveRangeAttribute.Min.y);

            EditorGUI.CurveField(rect,
                                 property,
                                 curveRangeAttribute.Color == EColor.Clear
                                     ? Color.green
                                     : curveRangeAttribute.Color.GetColor(),
                                 curveRanges,
                                 label);

            EditorGUI.EndProperty();
        }
    }
}