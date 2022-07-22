namespace DreamBuilders
{
    public abstract class StateMachine : UnityEngine.MonoBehaviour
    {
        #region Fields

        protected IState _previousState;

        protected IState _currentState;

        public IState CurrentState
        {
            get => _currentState;
            set
            {
                if (value == null || _currentState == value) return;

                _currentState?.Exit();

                _previousState = _currentState;
                _currentState = value;

                _currentState?.Enter();
            }
        }

        #endregion

        #region Unity Methods

        private void FixedUpdate() => _currentState?.FixedTick();

        private void Update() => _currentState?.Tick();

        #endregion

        #region Methods

        public void RevertState()
        {
            if (_previousState == null) return;

            CurrentState = _previousState;
        }

        #endregion
    }
}