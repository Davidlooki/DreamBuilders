using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class StateMachineReader : MonoBehaviour
{
    #region Fields
    [SerializeField] private UnityEvent<Animator> _onStateMachineEnter = new UnityEvent<Animator>();
    [SerializeField] private UnityEvent<Animator> _onStateMachineExit = new UnityEvent<Animator>();
    [SerializeField] private UnityEvent<Animator> _onStateEnter = new UnityEvent<Animator>();
    [SerializeField] private UnityEvent<Animator> _onStateUpdate = new UnityEvent<Animator>();
    [SerializeField] private UnityEvent<Animator> _onStateMove = new UnityEvent<Animator>();
    [SerializeField] private UnityEvent<Animator> _onStateIK = new UnityEvent<Animator>();
    [SerializeField] private UnityEvent<Animator> _onStateExit = new UnityEvent<Animator>();

    private Animator _animator = null;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _animator = GetComponent<Animator>();

        StateMachineEvent _animatorHelper = _animator.GetBehaviour<StateMachineEvent>();

        if (!_animatorHelper)
            return;

        //_animatorHelper.m_OnStateMachineEnter.AddListener(() => _onStateMachineEnter?.Invoke(_animator));
        //_animatorHelper.m_OnStateMachineExit.AddListener(() => _onStateMachineExit?.Invoke(_animator));
        //_animatorHelper.m_OnStateEnter.AddListener(() => _onStateEnter?.Invoke(_animator));
        //_animatorHelper.m_OnStateUpdate.AddListener(() => _onStateUpdate?.Invoke(_animator));
        //_animatorHelper.m_OnStateMove.AddListener(() => _onStateMove?.Invoke(_animator));
        //_animatorHelper.m_OnStateIK.AddListener(() => _onStateIK?.Invoke(_animator));
        //_animatorHelper.m_OnStateExit.AddListener(() => _onStateExit?.Invoke(_animator));
    }
    #endregion

    #region Custom Methods
    #endregion
}