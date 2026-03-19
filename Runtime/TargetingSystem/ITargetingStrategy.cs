using DreamBuilders.AbilitySystem;

namespace DreamBuilders.TargetingSystem
{
    public interface ITargetingStrategy
    {
        ITargetContext Context { get; }
        TargetingManager TargetingManager { get; }
        bool IsTargeting { get; }

        void Start(ITargetContext context, TargetingManager targetingManager);
        void Update();
        void Cancel();
    }
    
    public interface ITargetingStrategy<T> where T: ITargetContext
    {
        T Context { get; }
        TargetingManager TargetingManager { get; }
        bool IsTargeting { get; }

        void Start(T abilityContext, TargetingManager targetingManager);
        void Update();
        void Cancel();
    }
}