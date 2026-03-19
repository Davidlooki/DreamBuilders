using DreamBuilders.AbilitySystem;
using DreamBuilders.CollectionSystem;
using UnityEngine;

namespace Match3
{
    [CreateAssetMenu(fileName = "NewAbilityCollection", menuName = "Ability Collection", order = 0)]
    public class AbilityCollection : Collection<AbilityData> { }
}