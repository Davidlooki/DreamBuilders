using JetBrains.Annotations;
using UnityEngine;

namespace DreamBuilders.StatsSystem
{
    [System.Serializable]
    public class StatModifier : IStatModifier 
    {
        [field: SerializeField] public Object Source { get; set; }
        [field: SerializeField] public ModifierType ModifierType { get; set; }
        [field: SerializeField] public Stat StatTarget { get; set; }
        [field: SerializeField] public float Value { get; set; }

        public StatModifier([CanBeNull] Object source, ModifierType modifierType, Stat statTarget, float value)
        {
            Source = source;
            ModifierType = modifierType;
            StatTarget = statTarget;
            Value = value;
        }
    }
}