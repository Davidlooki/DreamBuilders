namespace DreamBuilders
{
    public interface IStateContext
    {
        #region Fields

        public IState CurrentState { get; }
        public IState PreviousState { get; }

        #endregion

        #region Methods

        public void SetState(IState state);
        public void RevertState();

        #endregion
    }

    public interface IStateContext<T> where T : IStateContext<T>
    {
        #region Fields

        public IState<T> CurrentState { get; }
        public IState<T> PreviousState { get; }

        #endregion

        #region Methods

        public void SetState(IState<T> state);
        public void RevertState();

        #endregion
    }
}