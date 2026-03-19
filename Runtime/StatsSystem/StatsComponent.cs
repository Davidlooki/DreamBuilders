using System;
using System.Collections.Generic;
using System.Linq;
using DreamBuilders;
using DreamBuildersLibs;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DreamBuilders.StatsSystem
{
    [Serializable]
    public class StatsComponent : MonoBehaviour
    {
        public IReadOnlyDictionary<IStat, StatInfo> StatsValues => _statsValues;
        private readonly Dictionary<IStat, StatInfo> _statsValues = new();

        public void Initialize(IStatModifier[] statsBase, IStatModifier[] statsBonus = null)
        {
            if (statsBase.IsNullOrEmpty())
                return;

            _statsValues.Clear();

            // Create entries for StatInfo and referred Stat
            statsBase.ForEachDo(modifier =>
                _statsValues.Add(modifier.StatTarget, new StatInfo(this, modifier.StatTarget, statsBonus, statsBonus)));

            // Bind StatInfo to source Stat changes
            foreach (var targetEntry in _statsValues)
            {
                foreach (var sourceEntry in statsBase)
                {
                    if (_statsValues[targetEntry.Key].ModifiersList
                        .All(x => x.Source != sourceEntry.StatTarget
                                  //Avoid self-binding
                                  || x.Source == (Object)targetEntry.Key)) continue;

                    targetEntry.Value.Bind(_statsValues[sourceEntry.StatTarget]);
                }
            }

            UpdateValues();
        }

        public void AddModifiers(
            IEnumerable<IStatModifier> modifiers,
            [CanBeNull] out IEnumerable<IStatModifier> failedModifiers,
            float duration = 0f
        )
        {
            failedModifiers = modifiers.Where(x =>
                //Remove null modifiers
                x == null
                //Remove modifiers where Stat Target is not in Stats List
                || !_statsValues.ContainsKey(x.StatTarget));

            modifiers.Except(failedModifiers)
                .ForEachDo(x =>
                {
                    _statsValues[x.StatTarget].AddModifier(x, false);

                    if (x.Duration > 0f)
                        this.Invoke(() => TryRemoveModifier(x), x.Duration);
                });

            UpdateValues();
        }

        public bool TryAddModifier(IStatModifier statModifier)
        {
            if (statModifier == null || !_statsValues.TryGetValue(statModifier.StatTarget, out var statInfo))
                return false;

            statInfo.AddModifier(statModifier);

            if (statModifier.Duration > 0f)
                this.Invoke(() => TryRemoveModifier(statModifier), statModifier.Duration);

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
                || !_statsValues.ContainsKey(x.StatTarget));

            modifiers.Except(failedModifiers)
                .ForEachDo(x => _statsValues[x.StatTarget].RemoveModifier(x, false));

            UpdateValues();
        }

        public bool TryRemoveModifier(IStatModifier statModifier)
        {
            if (statModifier == null || !_statsValues.TryGetValue(statModifier.StatTarget, out _)) return false;

            _statsValues[statModifier.StatTarget].RemoveModifier(statModifier);

            return true;
        }

        public bool TryRemoveAllModifiersFromSource(
            [CanBeNull] Object source,
            out IEnumerable<IStatModifier> removedStatModifiers
        )
        {
            removedStatModifiers = Enumerable.Empty<IStatModifier>();

            foreach (var statInfo in _statsValues.Values)
            {
                _statsValues[statInfo.Stat].RemoveAllModifiersFromSource(source, out var statRemovedModifiers, false);

                removedStatModifiers =
                    removedStatModifiers.Concat(statRemovedModifiers ?? Enumerable.Empty<IStatModifier>());
            }

            UpdateValues();

            return !removedStatModifiers.IsNullOrEmpty();
        }

        public void UpdateValues()
        {
            for (int i = 0; i < _statsValues.Keys.Count; i++)
                _statsValues.Values.ElementAt(i).UpdateValue();
        }
    }
}