using DreamBuilders.CollectionSystem;
using UnityEngine;

namespace DreamBuilders.StatsSystem
{
    /// <summary>
    /// Asset that hold a collection of <see cref="DreamBuilders.StatsSystem.IStat"/> 
    /// </summary>
    [CreateAssetMenu(fileName = "New Stat List", menuName = "Stat List", order = 0)]
    public class StatList : Collection<Stat> { }
}