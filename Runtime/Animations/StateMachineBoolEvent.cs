using UnityEngine;

namespace DreamBuilders.Animations
{
    public class StateMachineBoolEvent : StateMachineEventArgs<bool>
    {
        public override void Raise(Animator animator) => animator.SetBool(ParameterName, Value);
    }
}