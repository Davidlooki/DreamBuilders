using DreamBuilders.Interfaces;

namespace DreamBuilders.AbilitySystem
{
    public interface IAbilityEffect : IEffect
    {
        float Delay { get; }
        void Apply(IAbilityOwner source, IAbilityTargetable target);
    }
}