using System;
using UnityEngine;

namespace DreamBuilders.StatsSystem
{
    [Serializable]
    public class FromStat : IDisposable
    {
        [field: SerializeField] public Stat Source { get; protected set; }
        public float Value { get; protected set; } = 0;

        private StatInfo _sourceStatInfo;

        public FromStat(Stat source) => Source = source;

        public FromStat(Stat source, StatsComponent statsComponent)
        {
            Source = source;
            TryBindTo(statsComponent);
        }

        public bool TryBindTo(StatsComponent statsComponent)
        {
            if (_sourceStatInfo != null)
                _sourceStatInfo.OnValueChanged -= OnSourceValueChanged;

            if (!statsComponent.StatValues.TryGetValue(Source, out var statInfo)) return false;

            _sourceStatInfo = statInfo;
            _sourceStatInfo.OnValueChanged += OnSourceValueChanged;
            OnSourceValueChanged();

            return true;
        }

        private void OnSourceValueChanged() => Value = _sourceStatInfo.TotalValue;

        public void Dispose() => _sourceStatInfo.OnValueChanged -= OnSourceValueChanged;
    }
}