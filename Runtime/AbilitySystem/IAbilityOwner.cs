using UnityEngine;

namespace DreamBuilders.AbilitySystem
{
    public interface IAbilityOwner<T> where T : MonoBehaviour, IAbilityOwner<T>
    {
        AbilityExecutor<T> Executor { get; }
    }
}