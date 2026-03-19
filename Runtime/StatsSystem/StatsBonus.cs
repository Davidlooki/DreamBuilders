using System.Collections.Generic;
using UnityEngine;

namespace DreamBuilders.StatsSystem
{
    /// <summary>
    /// Contains a reference list of bonus from Stats (source) to Stats (target).
    /// </summary>
    /// <remarks>
    /// For each Source Base Value, the corresponding Target will receive a bonus according to the defined modifier.
    /// </remarks>
    [CreateAssetMenu(fileName = "NewStatsBonus", menuName = "Stats Bonus", order = 0)]
    public class StatsBonus : ScriptableObject, IStatsDefinition
    {
        [field: SerializeField, Required] public StatList StatsList { get; protected set; }

        /// <summary>
        /// List containing all bonus modifiers from source stats to target stats.
        /// </summary>
        public IReadOnlyList<StatModifier> Modifiers => _modifiers.AsReadOnly();

        [SerializeField, HideInInspector] private List<StatModifier> _modifiers = new();

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (StatsList == null)
                return;

            // Remove modifiers that...
            _modifiers.RemoveAll(modifier =>
                //...the target is null
                !modifier.StatTarget
                //...the target is not in StatList
                || !StatsList.Contains((Stat)modifier.StatTarget)
                //...source is null
                || !modifier.Source
                //...source is not a Stat type
                || modifier.Source is not Stat stat
                //...is not listed in StatList
                || !StatsList.Contains(stat));
        }
#endif
    }
}