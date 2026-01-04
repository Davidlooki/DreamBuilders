using System.Collections.Generic;

namespace DreamBuilders.BehaviourTree
{
    public class Node
    {
        public readonly string Name;
        public readonly int Priority;

        protected IReadOnlyCollection<Node> Childrens => _childrens.AsReadOnly();
        protected readonly List<Node> _childrens = new();
        protected int _currentChild;

        public Node(string name = "Node", int priority = 0)
        {
            this.Name = name;
            this.Priority = priority;
        }

        public void AddChild(Node child) => _childrens.Add(child);

        public virtual Status Process() => _childrens[_currentChild].Process();

        public virtual void Reset()
        {
            _currentChild = 0;
            foreach (var child in _childrens) { child.Reset(); }
        }
    }
}