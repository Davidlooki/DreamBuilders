namespace DreamBuilders
{
    public abstract class StateMachine : UnityEngine.MonoBehaviour, IStateContext
    {
        #region Fields

        public IState PreviousState { get; protected set; }
        public IState CurrentState { get; protected set; }

        #endregion

        #region Unity Methods

        private void FixedUpdate() => CurrentState?.FixedTick(this);

        private void Update() => CurrentState?.Tick(this);

        #endregion

        #region Methods

        public void SetState(IState state)
        {
            if (state == null || CurrentState == state) return;

            CurrentState?.Exit(this);

            PreviousState = CurrentState;
            CurrentState = state;

            CurrentState?.Enter(this);
        }

        public void RevertState()
        {
            if (PreviousState == null) return;

            CurrentState = PreviousState;
        }

        #endregion
    }
    
    public abstract class StateMachine<T> : UnityEngine.MonoBehaviour, IStateContext<T> where T: StateMachine<T>
    {
        private IState<T> _currentState;

        #region Fields

        IState<T> IStateContext<T>.CurrentState => _currentState;

        public IState<T> PreviousState { get; protected set; }
        public IState<T> CurrentState { get; protected set; }

        #endregion

        #region Unity Methods

        private void FixedUpdate() => CurrentState?.FixedTick(this as T);

        private void Update() => CurrentState?.Tick(this as T);

        #endregion

        #region Methods

        public void SetState(IState<T> state)
        {
            if (state == null || CurrentState == state) return;

            CurrentState?.Exit(this as T);

            PreviousState = CurrentState;
            CurrentState = state;

            CurrentState?.Enter(this as T);
        }

        public void RevertState()
        {
            if (PreviousState == null) return;

            CurrentState = PreviousState;
        }

        #endregion
    }
}