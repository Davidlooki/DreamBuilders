using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DreamBuilders.Editor
{
    public class ReorderableListPropertyDrawer : SpecialCasePropertyDrawerBase
    {
        public static readonly ReorderableListPropertyDrawer Instance = new ReorderableListPropertyDrawer();

        private readonly Dictionary<string, ReorderableList> _reorderableListsByPropertyName =
            new Dictionary<string, ReorderableList>();

        private GUIStyle _labelStyle;

        private GUIStyle GetLabelStyle() =>
            _labelStyle ?? new GUIStyle(EditorStyles.boldLabel) {richText = true};


        private string GetPropertyKeyName(SerializedProperty property) =>
            property.serializedObject.targetObject.GetInstanceID() + "." + property.name;

        protected override float GetPropertyHeight_Internal(SerializedProperty property)
        {
            if (!property.isArray) return EditorGUI.GetPropertyHeight(property, true);

            string key = GetPropertyKeyName(property);

            if (_reorderableListsByPropertyName.TryGetValue(key, out ReorderableList reorderableList) == false)
                return 0;

            return reorderableList.GetHeight();
        }

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (property.isArray)
            {
                string key = GetPropertyKeyName(property);

                ReorderableList reorderableList = null;
                if (!_reorderableListsByPropertyName.ContainsKey(key))
                {
                    reorderableList = new ReorderableList(property.serializedObject, property, true, true, true, true)
                    {
                        drawHeaderCallback = r =>
                        {
                            EditorGUI.LabelField(r, $"{label.text}: {property.arraySize}",
                                                 GetLabelStyle());
                            HandleDragAndDrop(r, reorderableList);
                        },

                        drawElementCallback = (r, index, _, _) =>
                        {
                            SerializedProperty element = property.GetArrayElementAtIndex(index);
                            r.y += 1.0f;
                            r.x += 10.0f;
                            r.width -= 10.0f;

                            EditorGUI.PropertyField(new Rect(r.x, r.y, r.width, EditorGUIUtility.singleLineHeight),
                                                    element, true);
                        },

                        elementHeightCallback = (index) =>
                            EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(index)) + 4.0f
                    };

                    _reorderableListsByPropertyName[key] = reorderableList;
                }

                reorderableList = _reorderableListsByPropertyName[key];

                if (rect == default)
                    reorderableList.DoLayoutList();
                else
                    reorderableList.DoList(rect);
            }
            else
            {
                const string message = nameof(ReorderableListAttribute) + " can be used only on arrays or lists";

                DreamBuildersEditorGUI.HelpBox_Layout(message, MessageType.Warning, property.serializedObject.targetObject);
                EditorGUILayout.PropertyField(property, true);
            }
        }

        public void ClearCache() =>
            _reorderableListsByPropertyName.Clear();

        private Object GetAssignableObject(Object obj, ReorderableList list)
        {
            Type listType = PropertyUtility.GetPropertyType(list.serializedProperty);
            Type elementType = ReflectionUtility.GetListElementType(listType);

            if (elementType == null)
                return null;

            Type objType = obj.GetType();

            if (elementType.IsAssignableFrom(objType))
                return obj;

            if (objType != typeof(GameObject)) return null;

            if (!typeof(Transform).IsAssignableFrom(elementType))
                return typeof(MonoBehaviour).IsAssignableFrom(elementType)
                    ? ((GameObject) obj).GetComponent(elementType)
                    : null;

            Transform transform = ((GameObject) obj).transform;

            if (elementType != typeof(RectTransform)) return transform;

            RectTransform rectTransform = transform as RectTransform;
            return rectTransform;
        }

        private void HandleDragAndDrop(Rect rect, ReorderableList list)
        {
            Event currentEvent = Event.current;
            bool usedEvent = false;

            switch (currentEvent.type)
            {
                case EventType.DragExited:
                {
                    if (GUI.enabled)
                        HandleUtility.Repaint();

                    break;
                }
                case EventType.DragUpdated:
                case EventType.DragPerform:
                {
                    if (rect.Contains(currentEvent.mousePosition) && GUI.enabled)
                    {
                        // Check each single object, so we can add multiple objects in a single drag.
                        bool didAcceptDrag = false;
                        Object[] references = DragAndDrop.objectReferences;
                        foreach (Object obj in references)
                        {
                            Object assignableObject = GetAssignableObject(obj, list);

                            if (assignableObject == null) continue;

                            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                            if (currentEvent.type != EventType.DragPerform) continue;

                            list.serializedProperty.arraySize++;
                            int arrayEnd = list.serializedProperty.arraySize - 1;
                            list.serializedProperty.GetArrayElementAtIndex(arrayEnd).objectReferenceValue =
                                assignableObject;
                            didAcceptDrag = true;
                        }

                        if (didAcceptDrag)
                        {
                            GUI.changed = true;
                            DragAndDrop.AcceptDrag();
                            usedEvent = true;
                        }
                    }

                    break;
                }
            }

            if (usedEvent)
                currentEvent.Use();
        }
    }
}