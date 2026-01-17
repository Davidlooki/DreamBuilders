using UnityEngine;

namespace DreamBuilders.AbilitySystem
{
    public class AbilityExecutor<T> where T : MonoBehaviour, IAbilityOwner<T>
    {
        private readonly T _owner;
        public AbilityExecutor(T owner) => _owner = owner;

        // private void SpawnVfx()
        // {
        //     if (!CurrentAbility.VfxPrefab)
        //         return;
        //
        //     var vfxInstance =
        //         Object.Instantiate(CurrentAbility.VfxPrefab, Owner.transform.position, Quaternion.identity);
        // }

        public void Execute(AbilityData ability, GameObject target)
        {
            foreach (var effect in ability.Effects)
                effect.Execute(_owner, target);
        }
    }
}