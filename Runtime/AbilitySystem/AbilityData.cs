using DreamBuilders.CollectionSystem;
using DreamBuilders.TargetingSystem;
using DreamBuildersLibs;
using UnityEngine;

namespace DreamBuilders.AbilitySystem
{
    [CreateAssetMenu(fileName = "NewAbilityData", menuName = "Ability System/Ability Data", order = 0)]
    public class AbilityData : CollectionEntry
    {
        [field: SerializeField, Min(0)] public float CastTime { get; protected set; } = 0;
        [field: SerializeField, Min(0)] public float FixedCooldown { get; protected set; } = 0;
        [field: SerializeField] public GameObject VfxPrefab { get; protected set; }
        [field: SerializeReference, Subclass] public ITargetingStrategy TargetingStrategy { get; protected set; }
        [field: SerializeReference, Subclass] public IAbilityEffect[] Effects { get; protected set; }

        public void Execute(IAbilityOwner source, IAbilityTargetable target)
        {
            HandleVFX(target);
            Effects.ForEachDo(effect => effect.Apply(source, target));
        }

        void HandleVFX(IAbilityTargetable target)
        {
            if (target is not MonoBehaviour targetMb) return;

            // if (castVfx != null) {
            //     Object.Instantiate(castVfx, targetMb.transform.position.Add(y:2), Quaternion.identity);
            // }
            //
            // if (runningVfx != null) {
            //     var runningVfxInstance = Object.Instantiate(runningVfx, targetMb.transform);
            //     Object.Destroy(runningVfxInstance, 3f);
            // }
        }
    }
}