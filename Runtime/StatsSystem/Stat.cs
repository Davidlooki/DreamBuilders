using DreamBuilders.CollectionSystem;
using UnityEngine;

namespace DreamBuilders.StatsSystem
{
    /// <summary>
    /// Basic module of Stat System. Contain information about a Stat, for references.
    /// </summary>
    [CreateAssetMenu(fileName = "New Stat", menuName = "Stat", order = 0)]
    public class Stat : CollectionEntry, IStat
    {
        [field: SerializeField] public string ShortName { get; protected set; }
        [field: SerializeField] public float MaxValue { get; protected set; } = float.MaxValue;
        [field: SerializeField] public float MinValue { get; protected set; } = float.MinValue;
        [field: SerializeField] public bool IsResource { get; protected set; }

        protected override void OnNameChanged()
        {
            base.OnNameChanged();

            var clampNameLength = Mathf.Clamp(Name.Length, 0, 3);

            if (string.IsNullOrEmpty(ShortName))
                ShortName = Name[..Mathf.Clamp(Name.Length, 0, clampNameLength)].ToUpper();
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            MaxValue = Mathf.Clamp(MaxValue, MinValue, float.MaxValue);
            MinValue = Mathf.Clamp(MinValue, float.MinValue, MaxValue);
        }
    }
}