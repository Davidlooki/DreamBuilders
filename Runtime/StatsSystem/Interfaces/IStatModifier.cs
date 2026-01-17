using UnityEngine;

namespace DreamBuilders.StatsSystem
{
    public interface IStatModifier
    {
        Object Source { get; set; }
        ModifierType ModifierType { get; set; }
        Stat StatTarget { get; set; }
        float Value { get; set; }
    }
}