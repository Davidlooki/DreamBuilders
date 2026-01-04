namespace DreamBuilders.CollectionSystem
{
    public interface ICollectionEntry : IIdentity, INameable, IDescriptable
    {
        public UnityEngine.Sprite Icon { get; }
    }
}