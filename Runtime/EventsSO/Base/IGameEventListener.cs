namespace DreamBuilders.EventsSO
{
    public interface IGameEventListener<T>
    {
        void OnEventRaised(T item);
    }
}