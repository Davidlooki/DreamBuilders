using UnityEngine;

[RequireComponent(typeof(StateMachineEvent))]
public class StateMachineBoolEvent : StateMachineEventArgs<bool>
{
    #region Fields
    #endregion

    #region Unity Methods
    #endregion

    #region Custom Methods
    public override void Raise(Animator animator) => animator.SetBool(ParameterName, Value);
    #endregion
}