using System;
using System.Collections.Generic;
using System.Linq;
using DreamBuildersLibs;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DreamBuilders.StatsSystem
{
    /// <summary>
    /// Contain live information of a reference <see cref="Stat"/>, like Total value, Base value, Modifiers, etc.
    /// </summary>
    [Serializable]
    public class StatInfo
    {
        #region Fields

        public readonly IStat Stat;

        /// <summary>
        /// Total value of a Stat (Base + Modifiers values).
        /// </summary>
        public float TotalValue { get; protected set; } = 0;

        /// <summary>
        /// Value of a Stat without Modifiers.
        /// </summary>
        public float BaseValue
        {
            get => _baseValue;
            set
            {
                _baseValue = value;
                UpdateValue();
            }
        }

        protected float _baseValue = 0;

        public IReadOnlyCollection<IStatModifier> ModifiersList => _modifiers.AsReadOnly();
        protected readonly List<IStatModifier> _modifiers = new();

        public event Action OnValueChanged = () => { };
        protected readonly StatsComponent _owner;

        #endregion

        public StatInfo(StatsComponent owner, IStat stat)
        {
            _owner = owner;
            Stat = stat;

            // Combine IModifiers from StatsBase and StatsBonus
            _owner.StatsBase.Modifiers
                .Where(x => x.StatTarget == stat)
                .Concat(_owner.StatsBonus.BonusModifiers
                    .Where(x => x.StatTarget == stat))
                .ForEachDo(x => _modifiers.Add(x));
        }

        /// <summary>
        /// Must be called after all StatsBase and StatsBonus modifiers have been added to bind to source Stat changes.
        /// </summary>
        public void Bind(StatInfo info) => info.OnValueChanged += UpdateValue;

        #region Methods

        public void AddModifier(IStatModifier statModifier, bool updateValue = true)
        {
            if (statModifier.Value == 0) return;

            _modifiers.Add(statModifier);

            if (updateValue)
                UpdateValue();
        }

        public void RemoveModifier(IStatModifier statModifier, bool updateValue = true)
        {
            if (!_modifiers.Remove(statModifier)) return;

            if (updateValue)
                UpdateValue();
        }

        public void RemoveAllModifiersFromSource(
            [CanBeNull] Object source,
            [CanBeNull] out IEnumerable<IStatModifier> removedIModifiers,
            bool updateValue = true
        )
        {
            removedIModifiers = _modifiers.Where(modifier => modifier.Source == source).ToList();

            for (int i = 0; i < _modifiers.Count; i++)
                if (_modifiers[i].Source == source)
                    _modifiers.RemoveAt(i);

            if (updateValue)
                UpdateValue();
        }

        public void UpdateValue()
        {
            //Stat source from same stat means it's from the base stats
            _baseValue = ModifiersList.Where(IsBaseStat).Sum(modifier => modifier.Value);

            var lastValue = TotalValue;
            TotalValue = BaseValue;

            TotalValue += _modifiers
                .Where(modifier => !IsBaseStat(modifier) && modifier.ModifierType == ModifierType.Flat)
                .Sum(modifier => modifier.Value);

            TotalValue += _modifiers
                .Where(modifier => !IsBaseStat(modifier) && modifier.ModifierType == ModifierType.Additive)
                .Sum(modifier => _owner.StatValues[(IStat)modifier.Source].BaseValue * modifier.Value);

            TotalValue *= 1 + _modifiers
                .Where(modifier => !IsBaseStat(modifier) && modifier.ModifierType == ModifierType.Multiplicative)
                .Sum(modifier => _owner.StatValues[(IStat)modifier.Source].BaseValue * modifier.Value);

            TotalValue = Mathf.Clamp(TotalValue, Stat.MinValue, Stat.MaxValue);
            
            //Avoid stack overflow.
            if (!Mathf.Approximately(lastValue, TotalValue))
                OnValueChanged?.Invoke();

            return;

            bool IsBaseStat(IStatModifier stat) => stat.Source == Stat;
        }

        #endregion
    }
}