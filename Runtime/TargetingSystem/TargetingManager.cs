using UnityEngine;

namespace DreamBuilders.TargetingSystem
{
    public class TargetingManager : MonoBehaviour
    {
        //[SerializeField] private InputReader _inputReader;
        //[SerializeField] private Camera _cam;

        private ITargetingStrategy _currentStrategy;

        public void Update()
        {
            if (_currentStrategy != null && _currentStrategy.IsTargeting)
                _currentStrategy.Update();
        }

        public void SetCurrentStrategy(ITargetingStrategy strategy) => _currentStrategy = strategy;
        public void ClearCurrentStrategy() => _currentStrategy = null;
    }
}