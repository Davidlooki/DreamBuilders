using UnityEngine;

[RequireComponent(typeof(StateMachineEvent))]
public class StateMachineBoolEvent : StateMachineEventArgs<bool>
{
    public override void Raise(Animator animator) => animator.SetBool(ParameterName, Value);
}