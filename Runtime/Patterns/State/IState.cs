namespace DreamBuilders
{
    public interface IState
    {
        //Automaticly called in State Machine. Allows delay flow if desired.
        void Enter();

        //Allows simulation of Update without MonoBehaviour attached.
        void Tick();

        //Allows simulation of FixedUpdate without MonoBehaviour attached.
        void FixedTick();
        
        //Automaticly called in State Machine. Allows delay flow if desired.
        void Exit();
    }
}