using System.Collections.Generic;
using System.Linq;
using DreamBuildersLibs;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DreamBuilders.StatsSystem
{
    public class StatsComponent : MonoBehaviour
    {
        [field: SerializeField, Required] public StatsBase StatsBase { get; protected set; }
        [field: SerializeField] public StatsBonus StatsBonus { get; protected set; }

        public IReadOnlyDictionary<IStat, StatInfo> StatValues => _statValues;
        private readonly Dictionary<IStat, StatInfo> _statValues = new();

        protected void OnEnable()
        {
            // Create entries for StatInfo and referred Stat
            StatsBase.StatsList.ForEachDo(stat => _statValues.Add(stat, new StatInfo(this, stat)));

            // Bind StatInfo to source Stat changes
            foreach (var targetEntry in _statValues)
            {
                foreach (var sourceEntry in StatsBase.StatsList)
                {
                    if (_statValues[targetEntry.Key].ModifiersList
                        .All(x => x.Source != sourceEntry
                                  //Avoid self-binding
                                  || x.Source == (Object)targetEntry.Key)) continue;

                    targetEntry.Value.Bind(_statValues[sourceEntry]);
                }
            }

            UpdateValues();
        }

        public void AddModifiers(
            IEnumerable<IStatModifier> modifiers,
            [CanBeNull] out IEnumerable<IStatModifier> failedModifiers
        )
        {
            failedModifiers = modifiers.Where(x =>
                //Remove null modifiers
                x == null
                //Remove modifiers where Stat Target is not in Stats List
                || !_statValues.ContainsKey(x.StatTarget));

            modifiers.Except(failedModifiers)
                .ForEachDo(x => _statValues[x.StatTarget].AddModifier(x, false));
            
            UpdateValues();
        }

        public bool TryAddModifier(IStatModifier statModifier)
        {
            if (statModifier == null || !_statValues.TryGetValue(statModifier.StatTarget, out var statInfo)) return false;

            statInfo.AddModifier(statModifier);

            return true;
        }
        
        public void RemoveModifiers(
            IEnumerable<IStatModifier> modifiers,
            [CanBeNull] out IEnumerable<IStatModifier> failedModifiers
        )
        {
            failedModifiers = modifiers.Where(x =>
                //Remove null modifiers
                x == null
                //Remove modifiers where Stat Target is not in Stats List
                || !_statValues.ContainsKey(x.StatTarget));

            modifiers.Except(failedModifiers)
                .ForEachDo(x => _statValues[x.StatTarget].RemoveModifier(x, false));
            
            UpdateValues();
        }

        public bool TryRemoveModifier(IStatModifier statModifier)
        {
            if (statModifier == null || !_statValues.TryGetValue(statModifier.StatTarget, out _)) return false;

            _statValues[statModifier.StatTarget].RemoveModifier(statModifier);

            return true;
        }

        public bool TryRemoveAllModifiersFromSource(
            [CanBeNull] Object source,
            [CanBeNull] out IEnumerable<IStatModifier> removedStatModifiers
        )
        {
            removedStatModifiers = Enumerable.Empty<IStatModifier>();

            foreach (var statInfo in _statValues.Values)
            {
                _statValues[statInfo.Stat].RemoveAllModifiersFromSource(source, out var statRemovedModifiers, false);

                if (!statRemovedModifiers.IsNullOrEmpty())
                    removedStatModifiers = removedStatModifiers.Concat(statRemovedModifiers);
            }

            UpdateValues();

            return !removedStatModifiers.IsNullOrEmpty();
        }

        public void UpdateValues()
        {
            for (int i = 0; i < _statValues.Keys.Count; i++)
                _statValues.Values.ElementAt(i).UpdateValue();
        }
    }
}