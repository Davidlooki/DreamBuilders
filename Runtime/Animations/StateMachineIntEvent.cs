using UnityEngine;

public class StateMachineIntEvent : StateMachineEventArgs<int>
{
    public override void Raise(Animator animator) => animator.SetInteger(ParameterName, Value);
}