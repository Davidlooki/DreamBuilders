namespace DreamBuilders.Patterns.StateMachine
{
    public abstract class StateMachine : IStateContext
    {
        public IState PreviousState { get; protected set; }
        public IState CurrentState { get; protected set; }

        private void FixedUpdate() => CurrentState?.FixedTick(this);

        private void Update() => CurrentState?.Tick(this);

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
    }

    public abstract class StateMachine<T> : IStateContext<T> where T : StateMachine<T>
    {
        public IState<T> PreviousState { get; protected set; }
        public IState<T> CurrentState { get; protected set; }

        private void FixedUpdate() => CurrentState?.FixedTick((T)this);

        private void Update() => CurrentState?.Tick((T)this);

        public void SetState(IState<T> state)
        {
            if (state == null || CurrentState == state) return;

            CurrentState?.Exit((T)this);

            PreviousState = CurrentState;
            CurrentState = state;

            CurrentState?.Enter((T)this);
        }

        public void RevertState()
        {
            if (PreviousState == null) return;

            CurrentState = PreviousState;
        }
    }
}