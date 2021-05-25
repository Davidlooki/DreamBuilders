using UnityEngine;

[System.Serializable]
public abstract class StateMachineEventArgs<T> : StateMachineBehaviour
{
    #region Fields
    public string ParameterName = string.Empty;
    public T Value = default;
    #endregion

    #region Constructors
    #endregion

    #region Custom Methods
    public abstract void Raise(Animator animator);
    #endregion
}