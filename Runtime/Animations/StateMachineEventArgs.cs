using UnityEngine;

[System.Serializable]
public abstract class StateMachineEventArgs<T> : StateMachineBehaviour
{
    public string ParameterName = string.Empty;
    public T Value = default;

    public abstract void Raise(Animator animator);
}