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
}