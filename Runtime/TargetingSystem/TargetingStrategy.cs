namespace DreamBuilders.TargetingSystem
{
    [System.Serializable]
    public abstract class TargetingStrategy : ITargetingStrategy
    {
        public ITargetContext Context { get; protected set; }
        public TargetingManager TargetingManager { get; protected set; }
        public bool IsTargeting { get; protected set; } = false;

        public abstract void Start(ITargetContext context, TargetingManager targetingManager);

        public virtual void Update() { }

        public virtual void Cancel() { }
    }

    [System.Serializable]
    public abstract class TargetingStrategy<T> : ITargetingStrategy<T> where T : ITargetContext
    {
        public T Context { get; protected set; }
        public TargetingManager TargetingManager { get; protected set; }
        public bool IsTargeting { get; protected set; } = false;

        public abstract void Start(T abilityContext, TargetingManager targetingManager);

        public virtual void Update() { }

        public virtual void Cancel() { }
    }
}