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
        [field: SerializeField, Min(0)] public float Duration { get; set; } = 0;

        public StatModifier(
            [CanBeNull] Object source,
            ModifierType modifierType,
            Stat statTarget,
            float value,
            float duration
        )
        {
            Source = source;
            ModifierType = modifierType;
            StatTarget = statTarget;
            Value = value;
            Duration = duration;
        }
    }
}