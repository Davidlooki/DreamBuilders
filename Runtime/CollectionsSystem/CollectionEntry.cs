using UnityEngine;

namespace DreamBuilders
{
    public abstract class CollectionEntry : ScriptableObject, ICollectionEntry
    {
        #region Fields

        [field: SerializeField] public Sprite Icon { get; protected set; } = null;

#if UNITY_EDITOR
        [SerializeField, OnValueChanged(nameof(GenerateId))]
        protected bool _notRandomId = true;

        [field: ShowIfAttribute(nameof(_notRandomId))]
#endif
        [field: SerializeField]
        public int Id { get; protected set; } = 0;

#if UNITY_EDITOR
        [field: OnValueChanged(nameof(OnNameChanged))]
#endif
        [field: SerializeField]
        public string Name { get; protected set; } = string.Empty;

        [field: SerializeField, TextArea(5, int.MaxValue)]
        public string Description { get; protected set; } = string.Empty;

        [field: SerializeField] public GameplayTag[] Tags { get; protected set; }

        #endregion

        #region Unity Methods

#if UNITY_EDITOR
        protected virtual void OnValidate() => OnNameChanged();
#endif

        #endregion

        #region Custom Methods

#if UNITY_EDITOR
        protected void GenerateId() => Id = new System.Random(Name.GetHashCode()).Next();
        protected void OnNameChanged()
        {
            if (string.IsNullOrEmpty(Name)) Name = name;
            if (!_notRandomId) GenerateId();
        }
#endif

        #endregion
    }
}
