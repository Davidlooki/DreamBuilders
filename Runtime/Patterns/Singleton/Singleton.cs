using UnityEngine;

namespace DreamBuilders
{
    [DisallowMultipleComponent]
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        #region Fields

        [SerializeField] protected bool _dontDestroyOnLoad = true;

        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<T>();

                if (_instance != null) return _instance;
                GameObject obj = new(typeof(T).Name);
                _instance = obj.AddComponent<T>();

                return _instance;
            }
        }

        #endregion

        #region Unity Methods

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;

                if (_dontDestroyOnLoad)
                    DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy() => _instance = null;

        #endregion
    }
}