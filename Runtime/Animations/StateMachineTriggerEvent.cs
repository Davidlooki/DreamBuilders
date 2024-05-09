using UnityEngine;
 
public class StateMachineTriggerEvent : StateMachineEventArgs<Void>
{
    public override void Raise(Animator animator) => animator.SetTrigger(ParameterName);
}