using UnityEngine;

namespace DreamBuilders.AbilitySystem
{
    [System.Serializable]
    public abstract class AbilityEffect : IAbilityEffect
    {
        [field: SerializeField] public float Delay { get; protected set; }

        public virtual void Apply(IAbilityOwner source, IAbilityTargetable target)
        {
            
        }
    }
}