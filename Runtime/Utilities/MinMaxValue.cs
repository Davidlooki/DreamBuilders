namespace DreamBuilders
{
    public class MinMaxValue
    {
        public float MinValue
        {
            get => _minValue;
            set
            {
                _minValue = System.Math.Clamp(value, float.MinValue, _maxValue);
                _value = System.Math.Clamp(_value, _minValue, _maxValue);
            }
        }

        private float _minValue;

        public float MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = System.Math.Clamp(value, _minValue, float.MaxValue);
                _value = System.Math.Clamp(_value, _minValue, _maxValue);
            }
        }

        private float _maxValue;

        public float Value
        {
            get => _value;
            set => _value = System.Math.Clamp(value, MinValue, MaxValue);
        }

        private float _value;

        public MinMaxValue(float minValue, float maxValue, float? value)
        {
            _minValue = minValue;
            _maxValue = maxValue;

            _value = value == null ? maxValue : System.Math.Clamp((float)value, MinValue, MaxValue);
        }
    }
}