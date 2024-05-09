namespace DreamBuilders
{
    public interface ICollectionEntry : IIdentity, INameable, IDescriptable
    {
        public UnityEngine.Sprite Icon { get; }
    }
}