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
}