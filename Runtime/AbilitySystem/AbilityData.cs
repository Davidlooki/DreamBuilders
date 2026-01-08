using DreamBuilders.CollectionSystem;
using UnityEngine;

namespace DreamBuilders.AbilitySystem
{
    [CreateAssetMenu(fileName = "NewAbilityData", menuName = "Ability System/Ability Data", order = 0)]
    public class AbilityData : CollectionEntry
    {
        [field: SerializeField, Min(0)] public float CastTime { get; protected set; } = 0;
        [field: SerializeField, Min(0)] public float FixedCooldown { get; protected set; } = 0;
        [field: SerializeField] public GameObject VfxPrefab { get; protected set; }
        [field: SerializeReference] public IAbilityEffect[] Effects { get; protected set; }
    }
}