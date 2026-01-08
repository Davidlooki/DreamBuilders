using UnityEngine;

namespace DreamBuilders.AbilitySystem
{
    public interface IAbilityEffect
    { 
        void Execute<T>(T source, GameObject target) where T : MonoBehaviour, IAbilityOwner<T>;
    }
}