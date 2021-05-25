using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

public class StateMachineEvent : StateMachineBehaviour
{
    #region Fields
    public UnityEvent<Animator> m_OnStateMachineEnter = new UnityEvent<Animator>();
    public UnityEvent<Animator> m_OnStateMachineExit = new UnityEvent<Animator>();
    public UnityEvent<Animator> m_OnStateEnter = new UnityEvent<Animator>();
    public UnityEvent<Animator> m_OnStateUpdate = new UnityEvent<Animator>();
    public UnityEvent<Animator> m_OnStateMove = new UnityEvent<Animator>();
    public UnityEvent<Animator> m_OnStateIK = new UnityEvent<Animator>();
    public UnityEvent<Animator> m_OnStateExit = new UnityEvent<Animator>();
    #endregion

    #region Unity Methods
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        m_OnStateMachineEnter?.Invoke(animator);
    }
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash, AnimatorControllerPlayable controller)
    {
        m_OnStateMachineEnter?.Invoke(animator);
    }

    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        m_OnStateMachineExit?.Invoke(animator);
    }
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash, AnimatorControllerPlayable controller)
    {
        m_OnStateMachineExit?.Invoke(animator);
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_OnStateEnter?.Invoke(animator);
    }
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        m_OnStateEnter?.Invoke(animator);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_OnStateUpdate?.Invoke(animator);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        m_OnStateUpdate?.Invoke(animator);
    }

    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_OnStateMove?.Invoke(animator);
    }
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        m_OnStateMove?.Invoke(animator);
    }

    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_OnStateIK?.Invoke(animator);
    }
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        m_OnStateIK?.Invoke(animator);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_OnStateExit?.Invoke(animator);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        m_OnStateExit?.Invoke(animator);
    }
    #endregion

    #region Custom Methods
    #endregion
}