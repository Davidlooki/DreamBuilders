using System.Collections;
using DreamBuilders.StatsSystem;
using DreamBuildersLibs;
using UnityEditor;
using UnityEngine;

namespace DreamBuilders.Editor.StatsSystem
{
    [CustomEditor(typeof(StatsBonus))]
    [CanEditMultipleObjects]
    public class StatsBonusEditor : DreamBuildersInspector
    {
        private StatsBonus _statsBonus;

        private new void OnEnable()
        {
            base.OnEnable();
            _statsBonus = target as StatsBonus;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!_statsBonus || !_statsBonus.StatsList || _statsBonus.StatsList.Count < 2) return;

            DrawBonusesStatsModifiers();
        }

        private void DrawBonusesStatsModifiers()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Bonus Modifiers");
            HorizontalLine();
            EditorGUILayout.EndHorizontal();

            _statsBonus.TryGetField("_modifiers", out var bonusFieldInfo);
            var bonusModifiers = bonusFieldInfo.GetValue(_statsBonus) as IList;

            if (bonusModifiers.Count == 0)
            {
                EditorGUILayout.HelpBox("No bonus modifiers.", MessageType.Info);
                AddBonusModifierButton(ref bonusModifiers);

                return;
            }

            var statsList = _statsBonus.StatsList;
            int statsCount = statsList.Count;

            // Prepare options for stats
            string[] statOptions = new string[statsCount];

            for (int s = 0; s < statsCount; s++)
                statOptions[s] = statsList[s].Name;

            for (int i = 0; i < bonusModifiers.Count; i++)
            {
                StatModifier bonus = bonusModifiers[i] as StatModifier;

                EditorGUILayout.BeginVertical("box");

                // Title + remove button
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(bonus.StatTarget != null ? bonus.StatTarget.Name : $"Bonus #{i}",
                    EditorStyles.boldLabel);

                GUILayout.FlexibleSpace();
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    Undo.RecordObject(_statsBonus, "Remove Bonus Modifier");
                    bonusModifiers.RemoveAt(i);
                    EditorUtility.SetDirty(_statsBonus);

                    break; // break to avoid invalidating the enumerator
                }

                EditorGUILayout.EndHorizontal();

                // Source dropdown
                int currentSourceIndex = 0;

                for (int j = 0; j < statsCount; j++)
                {
                    if (statsList[j] != bonus.Source) continue;

                    currentSourceIndex = j;

                    break;
                }

                EditorGUI.BeginChangeCheck();
                int chosenSource = EditorGUILayout.Popup("Source", currentSourceIndex, statOptions);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_statsBonus, "Change Bonus Source");
                    bonus.Source = statsList[chosenSource];
                    EditorUtility.SetDirty(_statsBonus);
                }

                // StatTarget dropdown (exclude Source)
                var targetLabels = new System.Collections.Generic.List<string>();
                var targetRealIndices = new System.Collections.Generic.List<int>();
                int currentTargetIndex = 0;
                for (int s = 0; s < statsCount; s++)
                {
                    if (statsList[s] == bonus.Source) continue; // exclude
                    targetRealIndices.Add(s);
                    targetLabels.Add(statsList[s].Name);
                    if (statsList[s] == bonus.StatTarget) currentTargetIndex = targetLabels.Count - 1;
                }

                EditorGUI.BeginChangeCheck();
                int chosenTarget = targetLabels.Count > 0
                    ? EditorGUILayout.Popup("Stat Target", currentTargetIndex >= 0 ? currentTargetIndex : 0,
                        targetLabels.ToArray())
                    : EditorGUILayout.Popup("Stat Target", 0, new[] { "<no target>" });

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_statsBonus, "Change Bonus Target");
                    bonus.StatTarget = targetLabels.Count > 0
                        ? statsList[targetRealIndices[chosenTarget]]
                        : null;

                    EditorUtility.SetDirty(_statsBonus);
                }

                // ModifierType
                EditorGUI.BeginChangeCheck();
                var newType = (ModifierType)EditorGUILayout.EnumPopup("Modifier Type", bonus.ModifierType);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_statsBonus, "Change Bonus Type");
                    bonus.ModifierType = newType;
                    EditorUtility.SetDirty(_statsBonus);
                }

                // Value
                EditorGUI.BeginChangeCheck();
                float newVal = EditorGUILayout.FloatField("Value", bonus.Value);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_statsBonus, "Change Bonus Value");
                    bonus.Value = newVal;
                    EditorUtility.SetDirty(_statsBonus);
                }

                EditorGUILayout.EndVertical();
            }

            AddBonusModifierButton(ref bonusModifiers);
        }

        public void AddBonusModifierButton(ref IList bonusModifiers)
        {
            // Add new bonus modifier button
            if (!GUILayout.Button("Add Bonus Modifier")) return;

            Undo.RecordObject(_statsBonus, "Add Bonus Modifier");

            var defaultSource = _statsBonus.StatsList.Count > 0 ? _statsBonus.StatsList[0] : null;
            var defaultTarget = _statsBonus.StatsList.Count > 1 ? _statsBonus.StatsList[1] : defaultSource;
            var newModifier = new StatModifier(defaultSource, ModifierType.Flat, defaultTarget, 0f);

            bonusModifiers.Add(newModifier);

            EditorUtility.SetDirty(_statsBonus);
        }
    }
}