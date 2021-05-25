using UnityEngine;

public class StateMachineIntEvent : StateMachineEventArgs<int>
{
    #region Fields
    #endregion

    #region Unity Methods
    #endregion

    #region Custom Methods
    public override void Raise(Animator animator) => animator.SetInteger(ParameterName, Value);
    #endregion
}