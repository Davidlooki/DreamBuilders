using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DreamBuilders.Editor
{
    [CustomPropertyDrawer(typeof(SubclassAttribute))]
    public class SubclassPropertyDrawer : PropertyDrawer
    {
        // Cache ReorderableLists per property path to keep foldout state and list state
        private static readonly Dictionary<string, ReorderableList> _lists = new();

        // Small padding constants
        private const float _ELEMENT_PADDING = 4f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property == null) return;

            // If this property represents an array/list of managed references
            if (property.isArray && property.propertyType == SerializedPropertyType.Generic)
            {
                var key = property.serializedObject.targetObject.GetInstanceID() + ":" + property.propertyPath;

                if (!_lists.TryGetValue(key, out var list))
                {
                    list = CreateReorderableList(property);
                    _lists[key] = list;
                }

                // keep reference up-to-date
                list.serializedProperty = property;

                list.DoList(position);

                return;
            }

            // Otherwise treat as a single managed reference
            DrawSingleManagedReference(position, property, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property == null) return base.GetPropertyHeight(property, label);

            if (property.isArray && property.propertyType == SerializedPropertyType.Generic)
            {
                var key = property.serializedObject.targetObject.GetInstanceID() + ":" + property.propertyPath;
                if (_lists.TryGetValue(key, out var list)) { return list.GetHeight(); }

                // fallback: create temporary list to measure height
                var tmp = CreateReorderableList(property);
                var h = tmp.GetHeight();

                return h;
            }

            // single managed reference
            float height = EditorGUIUtility.singleLineHeight;
            if (property.isExpanded) { height += _ELEMENT_PADDING + EditorGUI.GetPropertyHeight(property, true); }

            return height;
        }

        private ReorderableList CreateReorderableList(SerializedProperty arrayProperty)
        {
            var list = new ReorderableList(arrayProperty.serializedObject, arrayProperty, true, true, true, true)
            { drawHeaderCallback = rect => DrawListHeader(rect, arrayProperty) };

            list.elementHeightCallback = index =>
            {
                var elem = arrayProperty.GetArrayElementAtIndex(index);
                float headerH = EditorGUIUtility.singleLineHeight;
                float total = headerH + _ELEMENT_PADDING;
                if (elem != null && elem.isExpanded)
                {
                    float full = EditorGUI.GetPropertyHeight(elem, true);
                    float children = Math.Max(0f, full - headerH);
                    total += children + _ELEMENT_PADDING;
                }

                return total;
            };

            // Delegate element drawing to a helper to reduce complexity
            list.drawElementCallback = (rect, index, active, focused) => DrawListElement(rect, arrayProperty, index);

            list.onAddDropdownCallback = (rect, _) => OnAddElement(arrayProperty, rect);

            list.onRemoveCallback = rl =>
            {
                arrayProperty.DeleteArrayElementAtIndex(rl.index);
                arrayProperty.serializedObject.ApplyModifiedProperties();
            };

            return list;
        }

        // Header renderer for the reorderable list
        private void DrawListHeader(Rect rect, SerializedProperty arrayProperty)
        {
            EditorGUI.LabelField(rect, ObjectNames.NicifyVariableName(arrayProperty.displayName));
        }

        // Called when the add dropdown is invoked; shows the type picker
        private void OnAddElement(SerializedProperty arrayProperty, Rect rect)
        {
            var elementType = GetElementTypeFromField(arrayProperty);
            var hintAsm = arrayProperty.serializedObject.targetObject?.GetType()?.Assembly;
            ShowTypePicker(elementType, hintAsm, rect, t => AssignNewInstanceToArrayElement(arrayProperty, t));
        }

        private void AssignNewInstanceToArrayElement(SerializedProperty arrayProperty, Type type)
        {
            arrayProperty.serializedObject.Update();
            arrayProperty.arraySize++;
            var newElem = arrayProperty.GetArrayElementAtIndex(arrayProperty.arraySize - 1);
            newElem.managedReferenceValue = Activator.CreateInstance(type, Array.Empty<object>());
            arrayProperty.serializedObject.ApplyModifiedProperties();
        }

        // Draw a single element of the ReorderableList (extracted to reduce method size)
        private void DrawListElement(Rect rect, SerializedProperty arrayProperty, int index)
        {
            var elem = arrayProperty.GetArrayElementAtIndex(index);

            if (elem == null) return;

            float headerH = EditorGUIUtility.singleLineHeight;

            var headerRect = new Rect(rect.x, rect.y, rect.width, headerH);
            var childrenRect = new Rect(rect.x + 12, rect.y + headerH + _ELEMENT_PADDING / 2f, rect.width - 12,
                Math.Max(0, rect.height - headerH - _ELEMENT_PADDING));

            // Buttons
            var btnWRemove = 20f;
            var btnWType = 24f;
            var removeRect = new Rect(rect.x + rect.width - btnWRemove, headerRect.y, btnWRemove, headerH);
            var typeBtnRect = new Rect(rect.x + rect.width - btnWRemove - btnWType - 4f, headerRect.y, btnWType,
                headerH);

            int removeId = GUIUtility.GetControlID((arrayProperty.propertyPath + ":remove:" + index).GetHashCode(),
                FocusType.Passive);

            int typeId = GUIUtility.GetControlID((arrayProperty.propertyPath + ":type:" + index).GetHashCode(),
                FocusType.Passive);

            // Mouse handling (manual) to avoid interfering with foldout
            if (Event.current.type == EventType.MouseDown)
            {
                if (removeRect.Contains(Event.current.mousePosition))
                {
                    GUIUtility.hotControl = removeId;
                    Event.current.Use();
                }
                else if (typeBtnRect.Contains(Event.current.mousePosition))
                {
                    GUIUtility.hotControl = typeId;
                    Event.current.Use();
                }
            }

            if (Event.current.type == EventType.MouseUp)
            {
                if (GUIUtility.hotControl == removeId)
                {
                    GUIUtility.hotControl = 0;
                    if (removeRect.Contains(Event.current.mousePosition))
                    {
                        arrayProperty.DeleteArrayElementAtIndex(index);
                        arrayProperty.serializedObject.ApplyModifiedProperties();
                        Event.current.Use();

                        return;
                    }
                }
                else if (GUIUtility.hotControl == typeId)
                {
                    GUIUtility.hotControl = 0;
                    if (typeBtnRect.Contains(Event.current.mousePosition))
                    {
                        var baseType = GetElementTypeFromField(arrayProperty);
                        var hintAsm = arrayProperty.serializedObject.targetObject?.GetType()?.Assembly;
                        ShowTypePicker(baseType, hintAsm, typeBtnRect, t =>
                        {
                            arrayProperty.serializedObject.Update();
                            var targetElem = arrayProperty.GetArrayElementAtIndex(index);
                            targetElem.managedReferenceValue = Activator.CreateInstance(t, Array.Empty<object>());
                            arrayProperty.serializedObject.ApplyModifiedProperties();
                        });

                        Event.current.Use();
                    }
                }
            }

            // Buttons visuals
            GUI.Label(typeBtnRect, "T", EditorStyles.miniButton);
            GUI.Label(removeRect, "-", EditorStyles.miniButton);

            // Foldout header
            string displayName = GetManagedReferenceTypeName(elem) ?? "None";
            var foldoutRect = new Rect(headerRect.x, headerRect.y, headerRect.width - (btnWRemove + btnWType + 8f),
                headerRect.height);

            DrawFoldoutIfNotNull(elem, foldoutRect, displayName);

            // Draw children if expanded
            if (elem.isExpanded)
            {
                float full = EditorGUI.GetPropertyHeight(elem, true);
                float children = Math.Max(0f, full - headerH);
                var bodyRect = new Rect(childrenRect.x, childrenRect.y, childrenRect.width, children);
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(bodyRect, elem, GUIContent.none, true);
                EditorGUI.indentLevel--;
            }
        }

        // Resolve the element type for this field (array/list/backing-field-aware)
        private Type GetElementTypeFromField(SerializedProperty arrayProperty)
        {
            // Prefer fieldInfo when available
            if (fieldInfo != null)
            {
                var t = fieldInfo.FieldType;

                if (t.IsArray) return t.GetElementType();
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>))
                    return t.GetGenericArguments()[0];

                return t;
            }

            // Fallback: try to resolve the field on the target object by name
            try
            {
                var target = arrayProperty.serializedObject.targetObject;
                if (target != null)
                {
                    var targetType = target.GetType();
                    var fi = targetType.GetField(arrayProperty.name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    if (fi != null)
                    {
                        var ft = fi.FieldType;

                        if (ft.IsArray) return ft.GetElementType();
                        if (ft.IsGenericType && ft.GetGenericTypeDefinition() == typeof(List<>))
                            return ft.GetGenericArguments()[0];

                        return ft;
                    }
                }
            }
            catch (Exception) { }

            return typeof(object);
        }

        private void DrawSingleManagedReference(Rect position, SerializedProperty property, GUIContent label)
        {
            // draw header manually: buttons + foldout label
            float btnW = 22f;
            var typeRect = new Rect(position.x + position.width - btnW, position.y, btnW,
                EditorGUIUtility.singleLineHeight);

            var clearRect = new Rect(position.x + position.width - btnW - 26, position.y, 26,
                EditorGUIUtility.singleLineHeight);

            int singleTypeId =
                GUIUtility.GetControlID((fieldInfo.Name + ":singleType").GetHashCode(), FocusType.Passive);

            int singleClearId =
                GUIUtility.GetControlID((fieldInfo.Name + ":singleClear").GetHashCode(), FocusType.Passive);

            // MouseDown handling
            if (Event.current.type == EventType.MouseDown)
            {
                if (typeRect.Contains(Event.current.mousePosition))
                {
                    GUIUtility.hotControl = singleTypeId;
                    Event.current.Use();
                }
                else if (clearRect.Contains(Event.current.mousePosition))
                {
                    GUIUtility.hotControl = singleClearId;
                    Event.current.Use();
                }
            }

            // MouseUp handling
            if (Event.current.type == EventType.MouseUp)
            {
                if (GUIUtility.hotControl == singleTypeId)
                {
                    GUIUtility.hotControl = 0;
                    if (typeRect.Contains(Event.current.mousePosition))
                    {
                        var baseType = fieldInfo.FieldType;
                        var hintAsm3 = property.serializedObject.targetObject?.GetType()?.Assembly;

                        ShowTypePicker(baseType, hintAsm3, typeRect, t =>
                        {
                            property.serializedObject.Update();
                            property.managedReferenceValue = Activator.CreateInstance(t, Array.Empty<object>());
                            property.serializedObject.ApplyModifiedProperties();
                        });

                        Event.current.Use();
                    }
                }
                else if (GUIUtility.hotControl == singleClearId)
                {
                    GUIUtility.hotControl = 0;
                    if (clearRect.Contains(Event.current.mousePosition))
                    {
                        property.serializedObject.Update();
                        property.managedReferenceValue = null;
                        property.serializedObject.ApplyModifiedProperties();
                        Event.current.Use();
                    }
                }
            }

            // draw buttons visuals
            GUI.Label(typeRect, "T", EditorStyles.miniButton);
            GUI.Label(clearRect, "C", EditorStyles.miniButton);

            // draw foldout header
            var headerRect = new Rect(position.x, position.y, position.width - (btnW + 30f),
                EditorGUIUtility.singleLineHeight);

            string typeName = GetManagedReferenceTypeName(property) ?? "None";
            // draw the foldout or disabled label depending on whether a managed instance exists
            DrawFoldoutIfNotNull(property, headerRect, label.text + " (" + typeName + ")");

            // draw children only (no header)
            if (!property.isExpanded) return;

            float full = EditorGUI.GetPropertyHeight(property, true);
            float children = Math.Max(0f, full);
            var bodyRect = new Rect(position.x, position.y, position.width, children);

            EditorGUI.PropertyField(bodyRect, property, GUIContent.none, true);
        }

        // Helper: draw a foldout when the managed reference is not null; otherwise draw a disabled label and force collapsed
        private static void DrawFoldoutIfNotNull(SerializedProperty prop, Rect rect, string label)
        {
            bool isNull = false;
            try { isNull = prop.managedReferenceValue == null; }
            catch (Exception) { isNull = true; }

            if (isNull)
            {
                prop.isExpanded = false;
                using (new EditorGUI.DisabledScope(true)) { EditorGUI.LabelField(rect, label); }
            }
            else { prop.isExpanded = EditorGUI.Foldout(rect, prop.isExpanded, label, true); }
        }

        // Scans loaded assemblies and returns concrete non-abstract types assignable from baseType
        internal static List<Type> GetAssignableConcreteTypes(Type baseType)
        {
            var result = new List<Type>();

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.IsDynamic) continue;

                foreach (var t in GetTypesSafely(asm))
                {
                    try
                    {
                        if (IsConcreteAssignable(baseType, t)) { result.Add(t); }
                    }
                    catch (Exception)
                    {
                        // swallow per-assembly reflection issues
                    }
                }
            }

            // If nothing found, fallback to name matching across assemblies
            if (result.Count == 0 && baseType != null && !string.IsNullOrEmpty(baseType.Name))
            {
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (asm.IsDynamic) continue;

                    foreach (var t in GetTypesSafely(asm))
                    {
                        if (t == null) continue;
                        if (!t.IsClass) continue;
                        if (t.IsAbstract) continue;

                        try
                        {
                            if ((t.Name ?? string.Empty).IndexOf(baseType.Name, StringComparison.OrdinalIgnoreCase) >=
                                0)
                                result.Add(t);
                        }
                        catch (Exception)
                        {
                            /* ignore */
                        }
                    }
                }
            }

            result.Sort((a, b) => string.Compare(a.FullName, b.FullName, StringComparison.Ordinal));

            return result.Distinct().ToList();
        }

        // Helper: get assembly types safely
        private static IEnumerable<Type> GetTypesSafely(Assembly asm)
        {
            if (asm == null) yield break;
            Type[] types = null;
            try { types = asm.GetTypes(); }
            catch (ReflectionTypeLoadException ex) { types = ex.Types; }
            catch (Exception) { yield break; }

            if (types == null) yield break;
            foreach (var t in types)
            {
                if (t == null) continue;

                yield return t;
            }
        }

        // Helper: determine if a candidate type should be considered assignable/implementing the base type
        private static bool IsConcreteAssignable(Type baseType, Type candidate)
        {
            if (baseType == null || candidate == null) return false;
            if (!candidate.IsClass) return false;
            if (candidate.IsAbstract) return false;

            try
            {
                // Direct assignable covers derivation and interface implementation
                if (baseType.IsAssignableFrom(candidate)) return true;

                // If base is a generic type (e.g. IBase<T>), check generic interface/base definitions
                if (baseType.IsGenericType)
                {
                    var baseGenericDef = baseType.GetGenericTypeDefinition();

                    if ((from i in candidate.GetInterfaces()
                            where i.IsGenericType
                            select i.GetGenericTypeDefinition())
                        .Any(def => def == baseGenericDef)) { return true; }

                    var bt = candidate.BaseType;
                    while (bt != null && bt != typeof(object))
                    {
                        if (bt.IsGenericType && bt.GetGenericTypeDefinition() == baseGenericDef) return true;
                        bt = bt.BaseType;
                    }

                    return false;
                }

                // direct interface match
                if (baseType.IsInterface &&
                    (candidate.GetInterfaces().Any(i => i == baseType) ||
                     // fallback: interface name contains base name
                     (from i in candidate.GetInterfaces()
                         let name = i.Name
                         where name.Equals(baseType.Name,
                                   StringComparison.OrdinalIgnoreCase) ||
                               (i.FullName ?? string.Empty)
                               .IndexOf(baseType.Name,
                                   StringComparison.OrdinalIgnoreCase) >= 0
                         select i).Any()))
                    return true;
            }
            catch (Exception)
            {
                // ignore reflection problems for this candidate
            }

            return false;
        }

        // Simple popup type picker with search
        private class TypePickerPopup : PopupWindowContent
        {
            private readonly Type _baseType;
            private readonly Action<Type> _onPick;
            private List<Type> _all;
            private List<Type> _filtered;
            private string _search;
            private Vector2 _scroll;

            public TypePickerPopup(Type baseType, Action<Type> onPick, Assembly hintAssembly = null)
            {
                // normalize baseType: if array or generic List<>, get element type
                if (baseType == null) baseType = typeof(object);
                if (baseType.IsArray) baseType = baseType.GetElementType();
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(List<>))
                    baseType = baseType.GetGenericArguments()[0];

                _baseType = baseType ?? typeof(object);
                _onPick = onPick;
                _search = string.Empty;
                PopulateCandidates(hintAssembly);
            }

            // Populate _all and _filtered candidate lists, using hintAssembly and sensible fallbacks
            private void PopulateCandidates(Assembly hintAssembly)
            {
                _all = GetAssignableConcreteTypes(_baseType) ?? new List<Type>();
                _filtered = new List<Type>(_all);

                if ((_all == null || _all.Count == 0) && !string.IsNullOrEmpty(_baseType.Name))
                {
                    var alt = new List<Type>();
                    if (hintAssembly != null) TryAddTypesFromAssembly(hintAssembly, _baseType, alt);
                    try { TryAddTypesFromAssembly(_baseType.Assembly, _baseType, alt); }
                    catch (Exception) { }

                    if (alt.Count == 0)
                    {
                        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            if (asm.IsDynamic) continue;
                            TryAddTypesFromAssembly(asm, _baseType, alt, nameMatchOnly: true);
                        }
                    }

                    _filtered = alt;
                }
            }

            public override void OnGUI(Rect rect)
            {
                GUILayout.BeginVertical();
                // header (base type and count)
                var baseTypeName = _baseType?.FullName ?? "(unknown)";
                EditorGUILayout.LabelField("Base type: " + baseTypeName);
                EditorGUILayout.LabelField("Candidates: " + (_filtered?.Count ?? 0));

                EditorGUI.BeginChangeCheck();
                GUI.SetNextControlName("SubclassTypeSearch");
                _search = EditorGUILayout.TextField(_search);
                if (EditorGUI.EndChangeCheck()) UpdateFilter();

                GUILayout.Space(6);

                _scroll = EditorGUILayout.BeginScrollView(_scroll);
                DrawTypeList();
                EditorGUILayout.EndScrollView();

                GUILayout.EndVertical();

                if (Event.current.type == EventType.Repaint) EditorGUI.FocusTextInControl("SubclassTypeSearch");
            }

            // Draw the list of types (extracted for readability)
            private void DrawTypeList()
            {
                if (_filtered == null || _filtered.Count == 0)
                {
                    EditorGUILayout.HelpBox("No derived/implementing classes found for this base type.",
                        MessageType.Info);

                    return;
                }

                foreach (var t in _filtered)
                {
                    if (GUILayout.Button(t.FullName, EditorStyles.label))
                    {
                        _onPick?.Invoke(t);
                        editorWindow?.Close();

                        return;
                    }
                }
            }

            private void UpdateFilter()
            {
                if (string.IsNullOrEmpty(_search))
                {
                    _filtered = new List<Type>(_all ?? Enumerable.Empty<Type>());

                    return;
                }

                var s = _search.ToLowerInvariant();
                _filtered = (_all ?? Enumerable.Empty<Type>())
                    .Where(t => ((t?.FullName ?? string.Empty).ToLowerInvariant().Contains(s) ||
                                 (t?.Name ?? string.Empty).ToLowerInvariant().Contains(s)))
                    .ToList();
            }

            private static void TryAddTypesFromAssembly(
                Assembly asm,
                Type baseType,
                List<Type> outList,
                bool nameMatchOnly = false
            )
            {
                if (asm == null) return;

                foreach (var t in GetTypesSafely(asm))
                {
                    if (t == null) continue;
                    if (!t.IsClass) continue;
                    if (t.IsAbstract) continue;

                    try
                    {
                        if (!nameMatchOnly)
                        {
                            if (IsConcreteAssignable(baseType, t))
                            {
                                outList.Add(t);

                                continue;
                            }
                        }

                        // name match fallback
                        if (!string.IsNullOrEmpty(baseType.Name) &&
                            (t.Name ?? string.Empty).IndexOf(baseType.Name, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            outList.Add(t);
                        }
                    }
                    catch (Exception)
                    {
                        // ignore per-type reflection issues
                    }
                }
            }
        }

        // Helper to show the type picker popup
        private static void ShowTypePicker(Type baseType, Assembly hintAssembly, Rect position, Action<Type> onPick)
        {
            var popup = new TypePickerPopup(baseType, onPick, hintAssembly);
            PopupWindow.Show(position, popup);
        }

        // Helper to get the managed reference concrete type name safely
        private string GetManagedReferenceTypeName(SerializedProperty prop)
        {
            try
            {
                var val = prop.managedReferenceValue;

                return val != null ? val.GetType().Name : null;
            }
            catch (Exception) { return null; }
        }
    }
}