using System.Collections.Generic;
using DreamBuildersLibs;
using UnityEngine;

namespace DreamBuilders.StatsSystem
{
    /// <summary>
    /// Base class for defining a set of stat modifiers that can be applied to a character or entity.
    /// Use it only as a reference to build specific stats configurations.
    /// </summary>
    [CreateAssetMenu(fileName = "NewStatsBase", menuName = "Stats Base", order = 0)]
    public class StatsBase : ScriptableObject, IStatsDefinition
    {
#if UNITY_EDITOR
        [field: OnValueChanged(nameof(UpdateModifiers))]
#endif
        [field: SerializeField, Required]
        public StatList StatsList { get; protected set; }

        public IReadOnlyList<StatModifier> Modifiers => _modifiers.AsReadOnly();
        [SerializeField, HideInInspector] private List<StatModifier> _modifiers = new();

#if UNITY_EDITOR
        private void Reset()
        {
            _modifiers.Clear();
            StatsList = null;
        }

        public void UpdateModifiers()
        {
            if (!StatsList)
                return;

            // Remove modifiers that reference stats not in the StatsList
            _modifiers.RemoveAll(modifier => !StatsList.Contains(modifier.StatTarget));

            // Add entry for each stat in StatsList that doesn't have a modifier yet
            foreach (var stat in StatsList)
            {
                if (!_modifiers.Exists(modifier => modifier.StatTarget == stat))
                    _modifiers.Add(new StatModifier(stat, ModifierType.Flat, stat, 0f, 0f));
            }

            // Sort modifiers to match the order of stats in StatsList
            _modifiers.Sort((modifierA, modifierB) =>
                StatsList.IndexOf(modifierA.StatTarget)
                    .CompareTo(StatsList.IndexOf(modifierB.StatTarget)));
        }
#endif
    }
}