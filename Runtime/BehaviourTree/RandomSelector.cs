using System.Collections.Generic;
using System.Linq;

namespace DreamBuilders.BehaviourTree
{
    /// <summary>
    /// </summary>
    public class RandomSelector : PrioritySelector
    {
        protected virtual List<Node> SortChildren() => _childrens.Shuffle().ToList();
        public RandomSelector(string name) : base(name) { }
    }
}