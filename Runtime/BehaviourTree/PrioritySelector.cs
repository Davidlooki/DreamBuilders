using System.Collections.Generic;
using System.Linq;

namespace DreamBuilders.BehaviourTree
{
    /// <summary>
    /// </summary>
    public class PrioritySelector : Node
    {
        private List<Node> _sortedChildren;
        private List<Node> SortedChildren => _sortedChildren ??= SortChildren();

        public PrioritySelector(string name) : base(name) { }

        protected virtual List<Node> SortChildren() => _childrens.OrderByDescending(child => child.Priority).ToList();

        public override void Reset()
        {
            base.Reset();
            _sortedChildren = null;
        }

        public override Status Process()
        {
            foreach (var child in SortedChildren)
            {
                switch (child.Process())
                {
                    case Status.Running:
                        return Status.Running;

                    case Status.Success:
                        return Status.Success;

                    default:
                        continue;
                }
            }

            return Status.Failure;
        }
    }
}