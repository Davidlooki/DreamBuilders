namespace DreamBuilders.BehaviourTree
{
    public interface INodeStrategy : IStrategy
    {
        Status Process();
        void Reset() { }
    }
}