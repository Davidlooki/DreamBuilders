using System;

namespace DreamBuilders.StatsSystem
{
    [Serializable]
    public class FromStat : IDisposable
    {
        [field: UnityEngine.SerializeField] public Stat Source { get; protected set; }
        public float TotalValue { get; protected set; } = 0;

        public float CurrentValue
        {
            get => _currentValue;
            set => _currentValue = Math.Clamp(value, Source.MinValue, TotalValue);
        }

        private float _currentValue;

        private StatInfo _sourceStatInfo;

        public FromStat(Stat source) => Source = source;

        public FromStat(Stat source, StatsComponent statsComponent) : this(source) => TryBindTo(statsComponent);

        public bool TryBindTo(StatsComponent statsComponent)
        {
            if (_sourceStatInfo != null)
                _sourceStatInfo.OnValueChanged -= OnSourceValueChanged;

            if (!statsComponent.StatsValues.TryGetValue(Source, out var statInfo)) return false;

            _sourceStatInfo = statInfo;
            _sourceStatInfo.OnValueChanged += OnSourceValueChanged;
            OnSourceValueChanged();

            CurrentValue = TotalValue;
            
            return true;
        }

        private void OnSourceValueChanged() => TotalValue = _sourceStatInfo.TotalValue;

        public void Dispose() => _sourceStatInfo.OnValueChanged -= OnSourceValueChanged;
    }
}