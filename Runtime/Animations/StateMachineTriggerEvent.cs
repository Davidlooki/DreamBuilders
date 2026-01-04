using DreamBuilders.EventsSO;
using UnityEngine;

namespace DreamBuilders.Animations
{
    public class StateMachineTriggerEvent : StateMachineEventArgs<Void>
    {
        public override void Raise(Animator animator) => animator.SetTrigger(ParameterName);
    }
}