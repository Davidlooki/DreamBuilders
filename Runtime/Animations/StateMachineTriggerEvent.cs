using UnityEngine;
 
public class StateMachineTriggerEvent : StateMachineEventArgs<Void>
{
    #region Fields
    #endregion

    #region Unity Methods
    #endregion

    #region Custom Methods
    public override void Raise(Animator animator) => animator.SetTrigger(ParameterName);
    #endregion
}