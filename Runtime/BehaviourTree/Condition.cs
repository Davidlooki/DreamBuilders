using System;

namespace DreamBuilders.BehaviourTree
{
    public class Condition : INodeStrategy
    {
        private readonly Func<bool> _predicate;

        public Condition(Func<bool> predicate) => _predicate = predicate;
        public Status Process() => _predicate()? Status.Success : Status.Failure;
    }
}