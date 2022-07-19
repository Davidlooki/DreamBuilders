namespace DreamBuilders
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}