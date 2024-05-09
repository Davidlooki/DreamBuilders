using UnityEngine;
 
public class StateMachineFloatEvent : StateMachineEventArgs<float>
{
    public override void Raise(Animator animator) => animator.SetFloat(ParameterName, Value);
}