using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DreamBuilders.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Object), true)]
    public class DreamBuildersInspector : UnityEditor.Editor
    {
        #region Fields

        private List<SerializedProperty> _serializedProperties = new();
        private IEnumerable<FieldInfo> _nonSerializedFields;
        private IEnumerable<PropertyInfo> _nativeProperties;
        private IEnumerable<MethodInfo> _methods;
        private Dictionary<string, SavedBool> _foldouts = new();

        #endregion

        #region Constructors

        #endregion

        #region Unity Methods

        protected virtual void OnEnable()
        {
            _nonSerializedFields = ReflectionUtility.GetAllFields(
                                                                  target,
                                                                  f => f
                                                                       .GetCustomAttributes(typeof(ShowNonSerializedFieldAttribute),
                                                                        true).Length > 0);

            _nativeProperties = ReflectionUtility.GetAllProperties(
                                                                   target,
                                                                   p => p
                                                                        .GetCustomAttributes(typeof(ShowNativePropertyAttribute),
                                                                         true).Length > 0);

            _methods = ReflectionUtility.GetAllMethods(
                                                       target,
                                                       m => m.GetCustomAttributes(typeof(ButtonAttribute), true)
                                                             .Length > 0);
        }

        protected virtual void OnDisable() => ReorderableListPropertyDrawer.Instance.ClearCache();

        public override void OnInspectorGUI()
        {
            GetSerializedProperties(ref _serializedProperties);

            bool dreamBuildersAttribute =
                _serializedProperties.Any(p => PropertyUtility.GetAttribute<IPropertyAttribute>(p) != null);

            if (!dreamBuildersAttribute)
                DrawDefaultInspector();
            else
                DrawSerializedProperties();

            DrawNonSerializedFields();
            DrawNativeProperties();
            DrawButtons();
        }

        protected void GetSerializedProperties(ref List<SerializedProperty> outSerializedProperties)
        {
            outSerializedProperties.Clear();

            using SerializedProperty iterator = serializedObject.GetIterator();

            if (!iterator.NextVisible(true)) return;

            do outSerializedProperties.Add(serializedObject.FindProperty(iterator.name));
            while (iterator.NextVisible(false));
        }

        protected void DrawSerializedProperties()
        {
            serializedObject.Update();

            // Draw non-grouped serialized properties
            foreach (SerializedProperty property in GetNonGroupedProperties(_serializedProperties))
            {
                if (property.name.Equals("m_Script", System.StringComparison.Ordinal))
                {
                    using (new EditorGUI.DisabledScope(true))
                        EditorGUILayout.PropertyField(property);
                }
                else
                    DreamBuildersEditorGUI.PropertyField_Layout(property, true);
            }

            // Draw grouped serialized properties
            foreach (IGrouping<string, SerializedProperty> group in GetGroupedProperties(_serializedProperties))
            {
                IEnumerable<SerializedProperty> visibleProperties = group.Where(PropertyUtility.IsVisible);
                if (!visibleProperties.Any())
                    continue;

                DreamBuildersEditorGUI.BeginBoxGroup_Layout(group.Key);
                foreach (SerializedProperty property in visibleProperties)
                    DreamBuildersEditorGUI.PropertyField_Layout(property, true);

                DreamBuildersEditorGUI.EndBoxGroup_Layout();
            }

            // Draw foldout serialized properties
            foreach (IGrouping<string, SerializedProperty> group in GetFoldoutProperties(_serializedProperties))
            {
                IEnumerable<SerializedProperty> visibleProperties = group.Where(PropertyUtility.IsVisible);
                if (!visibleProperties.Any())
                    continue;

                if (!_foldouts.ContainsKey(group.Key))
                    _foldouts[group.Key] = new SavedBool($"{target.GetInstanceID()}.{group.Key}", false);

                _foldouts[group.Key].Value =
                    EditorGUILayout.Foldout(_foldouts[group.Key].Value, group.Key, true);

                if (!_foldouts[@group.Key].Value) continue;

                foreach (SerializedProperty property in visibleProperties)
                    DreamBuildersEditorGUI.PropertyField_Layout(property, true);
            }

            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawNonSerializedFields(bool drawHeader = false)
        {
            if (!_nonSerializedFields.Any()) return;

            if (drawHeader)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Non-Serialized Fields", GetHeaderGUIStyle());
                DreamBuildersEditorGUI.HorizontalLine(
                                                      EditorGUILayout.GetControlRect(false),
                                                      HorizontalLineAttribute.DefaultHeight,
                                                      HorizontalLineAttribute.DefaultColor.GetColor());
            }

            foreach (FieldInfo field in _nonSerializedFields)
                DreamBuildersEditorGUI.NonSerializedField_Layout(serializedObject.targetObject, field);
        }

        protected void DrawNativeProperties(bool drawHeader = false)
        {
            if (!_nativeProperties.Any()) return;

            if (drawHeader)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Native Properties", GetHeaderGUIStyle());
                DreamBuildersEditorGUI.HorizontalLine(
                                                      EditorGUILayout.GetControlRect(false),
                                                      HorizontalLineAttribute.DefaultHeight,
                                                      HorizontalLineAttribute.DefaultColor.GetColor());
            }

            foreach (PropertyInfo property in _nativeProperties)
                DreamBuildersEditorGUI.NativeProperty_Layout(serializedObject.targetObject, property);
        }

        protected void DrawButtons(bool drawHeader = false)
        {
            if (!_methods.Any()) return;

            if (drawHeader)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Buttons", GetHeaderGUIStyle());
                DreamBuildersEditorGUI.HorizontalLine(
                                                      EditorGUILayout.GetControlRect(false),
                                                      HorizontalLineAttribute.DefaultHeight,
                                                      HorizontalLineAttribute.DefaultColor.GetColor());
            }

            foreach (MethodInfo method in _methods)
                DreamBuildersEditorGUI.Button(serializedObject.targetObject, method);
        }

        private static IEnumerable<SerializedProperty> GetNonGroupedProperties(
            IEnumerable<SerializedProperty> properties) =>
            properties.Where(p => PropertyUtility.GetAttribute<IGroupPropertyAttribute>(p) == null);

        private static IEnumerable<IGrouping<string, SerializedProperty>> GetGroupedProperties(
            IEnumerable<SerializedProperty> properties) =>
            properties
                .Where(p => PropertyUtility.GetAttribute<BoxGroupAttribute>(p) != null)
                .GroupBy(p => PropertyUtility.GetAttribute<BoxGroupAttribute>(p).Name);

        private static IEnumerable<IGrouping<string, SerializedProperty>> GetFoldoutProperties(
            IEnumerable<SerializedProperty> properties) =>
            properties
                .Where(p => PropertyUtility.GetAttribute<FoldoutAttribute>(p) != null)
                .GroupBy(p => PropertyUtility.GetAttribute<FoldoutAttribute>(p).Name);

        private static GUIStyle GetHeaderGUIStyle() =>
            new GUIStyle(EditorStyles.centeredGreyMiniLabel)
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.UpperCenter
            };

        #endregion

        #region Methods

        #endregion
    }
}