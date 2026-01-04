using UnityEngine;

namespace DreamBuilders
{
    [CreateAssetMenu(fileName = "NewGameplayTag", menuName = "DreamBuilders/Gameplay Ability System/Tag",
                        order = 0)]
    public class GameplayTag : ScriptableObject
    {
#if UNITY_EDITOR
        [field: Delayed]
#endif
        [field: SerializeField]
        public string Name { get; protected set; }

        [field: SerializeField] public GameplayTag Parent { get; protected set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(Name)) Name = name;
        }
#endif

        public bool IsDescendantOf(GameplayTag other) => 
            Parent != null && (Parent == other || IsDescendantOf(other.Parent));

        //TODO: Add Descendants tree.
        public override string ToString()
        {
            return base.ToString();
        }
    }
}