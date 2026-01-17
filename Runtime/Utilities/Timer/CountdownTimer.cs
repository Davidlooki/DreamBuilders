namespace DreamBuilders.Utilities.Timer
{
    public class CountdownTimer : Timer
    {
        public CountdownTimer(float value) : base(value) { }

        public override void Tick(float deltaTime)
        {
            if (IsRunning && Time > 0) { Time -= deltaTime; }

            if (IsRunning && Time <= 0) { Stop(); }
        }

        public bool IsFinished => Time <= 0;

        public void Reset() => Time = _initialTime;

        public void Reset(float newTime)
        {
            _initialTime = newTime;
            Reset();
        }
    }
}