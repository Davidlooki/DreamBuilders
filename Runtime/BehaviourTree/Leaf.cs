namespace DreamBuilders.BehaviourTree
{
    /// <summary>
    /// A <see cref="Node"/> who have no childrens and instead have a behaviour to execute.
    /// </summary>
    public class Leaf : Node
    {
        private readonly INodeStrategy _strategy;

        public Leaf(string name, INodeStrategy strategy, int priority = 0) : base(name, priority) =>
            _strategy = strategy;

        public override Status Process() => _strategy.Process();
        public override void Reset() => _strategy.Reset();
    }
}