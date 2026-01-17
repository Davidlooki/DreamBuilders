using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands.Import;
using DreamBuilders.StatsSystem;
using DreamBuildersLibs;
using UnityEditor;
using UnityEngine;

namespace DreamBuilders.Editor.StatsSystem
{
    [CustomEditor(typeof(StatsBase))]
    [CanEditMultipleObjects]
    public class StatsBaseEditor : DreamBuildersInspector
    {
        // Keep a direct reference to the inspected StatsBase as requested
        private StatsBase _statsBase;

        private new void OnEnable()
        {
            base.OnEnable();
            _statsBase = target as StatsBase;

            if (_statsBase)
                _statsBase.UpdateModifiers();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!_statsBase || !_statsBase.StatsList) return;

            DrawStatsModifiers();
        }

        private void DrawStatsModifiers()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Base Modifiers");
            HorizontalLine();
            EditorGUILayout.EndHorizontal();

            _statsBase.TryGetField("_modifiers", out var modifierField);

            if (modifierField.GetValue(_statsBase) is not List<StatModifier> modifiers) return;

            for (var index = 0; index < modifiers.Count; index++)
            {
                var modifier = modifiers[index];
                EditorGUILayout.BeginVertical("box");

                // Show the stat target name as header
                string statName = modifier.StatTarget != null ? modifier.StatTarget.Name : "Modifier";
                EditorGUILayout.LabelField(statName, EditorStyles.boldLabel);

                // Source, ModifierType and StatTarget are readonly in the inspector
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField("Stat Target", modifier.StatTarget, typeof(Stat), false);
                EditorGUILayout.EnumPopup("Type", modifier.ModifierType);
                EditorGUI.EndDisabledGroup();

                // Editable Value field — modify directly on the target object (no SerializedObject)
                EditorGUI.BeginChangeCheck();
                float newValue = EditorGUILayout.FloatField("Value", modifier.Value);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_statsBase, $"Change modifier value ({statName})");
                    modifier.Value = newValue;
                    _statsBase.UpdateModifiers();
                    EditorUtility.SetDirty(_statsBase);
                }

                EditorGUILayout.EndVertical();
            }
        }
    }
}