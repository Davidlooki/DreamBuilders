namespace DreamBuilders
{
    public interface IState
    {
        //Automaticly called in State Machine. Allows delay flow if desired.
        public void Enter(IStateContext context);

        //Allows simulation of Update without MonoBehaviour attached.
        public void Tick(IStateContext context);

        //Allows simulation of FixedUpdate without MonoBehaviour attached.
        public void FixedTick(IStateContext context);

        //Automaticly called in State Machine. Allows delay flow if desired.
        public void Exit(IStateContext context);
    }

    public interface IState<in T> where T : IStateContext<T>
    {
        //Automaticly called in State Machine. Allows delay flow if desired.
        public void Enter(T context);

        //Allows simulation of Update without MonoBehaviour attached.
        public void Tick(T context);

        //Allows simulation of FixedUpdate without MonoBehaviour attached.
        public void FixedTick(T context);

        //Automaticly called in State Machine. Allows delay flow if desired.
        public void Exit(T context);
    }
}