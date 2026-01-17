using System;
using DreamBuilders.CollectionSystem;
using UnityEngine;

namespace DreamBuilders.StatsSystem
{
    public interface IStat : ICollectionEntry
    {
        string ShortName { get; }
        float MaxValue { get; }
        float MinValue { get; }
    }
}