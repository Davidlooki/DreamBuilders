using System;

namespace DreamBuilders.BehaviourTree
{
    public class Inverter : Node
    {
        public Inverter(string name) : base(name) { }

        public override Status Process()
        {
            switch (_childrens[0].Process())
            {
                case Status.Running:
                    return Status.Running;

                case Status.Failure:
                    Reset();

                    return Status.Success;

                default:
                    return Status.Failure;
            }
        }
    }
}