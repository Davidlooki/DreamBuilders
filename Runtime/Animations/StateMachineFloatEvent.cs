using UnityEngine;
 
public class StateMachineFloatEvent : StateMachineEventArgs<float>
{
    #region Fields
    #endregion

    #region Unity Methods
    #endregion

    #region Custom Methods
    public override void Raise(Animator animator) => animator.SetFloat(ParameterName, Value);
    #endregion
}