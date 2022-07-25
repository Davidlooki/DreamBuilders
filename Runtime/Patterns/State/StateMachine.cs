namespace DreamBuilders
{
    public abstract class StateMachine : UnityEngine.MonoBehaviour, IStateContext
    {
        #region Fields

        public IState PreviousState { get; protected set; }
        public IState CurrentState { get; protected set; }

        #endregion

        #region Unity Methods

        private void FixedUpdate() => CurrentState?.FixedTick();

        private void Update() => CurrentState?.Tick();

        #endregion

        #region Methods

        public void SetState(IState state)
        {
            if (state == null || CurrentState == state) return;

            CurrentState?.Exit();

            PreviousState = CurrentState;
            CurrentState = state;

            CurrentState?.Enter();
        }

        public void RevertState()
        {
            if (PreviousState == null) return;

            CurrentState = PreviousState;
        }

        #endregion
    }
}