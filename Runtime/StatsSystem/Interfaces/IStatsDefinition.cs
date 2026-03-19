using System.Collections.Generic;

namespace DreamBuilders.StatsSystem
{
    public interface IStatsDefinition
    {
        StatList StatsList { get; }
        IReadOnlyList<StatModifier> Modifiers { get; }
    }
}