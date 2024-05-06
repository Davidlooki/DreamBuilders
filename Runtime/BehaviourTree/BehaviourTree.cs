namespace DreamBuilders.BehaviourTree
{
    public class BehaviourTree : Node
    {
        public BehaviourTree(string name) : base(name) { }

        public override Status Process()
        {
            while (_currentChild < _childrens.Count)
            {
                var status = _childrens[_currentChild].Process();

                if (status != Status.Success)
                    return status;

                _currentChild++;
            }

            return Status.Success;
        }
    }
}