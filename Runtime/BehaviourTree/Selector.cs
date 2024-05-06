using System;

namespace DreamBuilders.BehaviourTree
{
    public class Selector : Node
    {
        public Selector(string name) : base(name) { }

        public override Status Process()
        {
            if (_currentChild < _childrens.Count)
            {
                switch (_childrens[_currentChild].Process())
                {
                    case Status.Running:
                        return Status.Running;

                    case Status.Success:
                        Reset();

                        return Status.Success;

                    default:
                        _currentChild++;

                        return Status.Running;
                }
            }

            Reset();

            return Status.Failure;
        }
    }
}