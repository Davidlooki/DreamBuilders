namespace DreamBuilders.BehaviourTree
{
    public class UntilFail : Node
    {
        public UntilFail(string name) : base(name) { }

        public override Status Process()
        {
            if (_childrens[0].Process() != Status.Failure) return Status.Running;

            Reset();

            return Status.Failure;
        }
    }
}