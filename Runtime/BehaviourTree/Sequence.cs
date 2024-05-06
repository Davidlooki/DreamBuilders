using System;

namespace DreamBuilders.BehaviourTree
{
    /// <summary>
    /// A node where all childrens must be successfull (like an logical "and").
    /// </summary>
    public class Sequence : Node
    {
        public Sequence(string name, int priority = 0) : base(name, priority) { }

        public override Status Process()
        {
            if (_currentChild < _childrens.Count)
            {
                switch (_childrens[_currentChild].Process())
                {
                    case Status.Running:
                        return Status.Running;

                    case Status.Failure:
                        return Status.Failure;

                    default:
                        _currentChild++;

                        return _currentChild == _childrens.Count ? Status.Success : Status.Running;
                }
            }

            Reset();

            return Status.Success;
        }
    }
}