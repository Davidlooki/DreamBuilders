using UnityEngine;

namespace DreamBuilders
{
	//public abstract class Singleton<T> where T : class, new()
	//{
	//	private static T _instance;
	//	public static T Instance
	//	{
	//		get
	//		{
	//			if(_instance == null)
	//				_instance = new T();

	//			return _instance;
	//		}
	//	}
	//}
	
	[DisallowMultipleComponent]
	public abstract class Singleton<T> : MonoBehaviour where T : Component
	{
		[SerializeField] protected bool _dontDestroyOnLoad = true;

		private static T _instance;
		public static T Instance
		{
			get
			{
				if(_instance == null)
				{
					_instance = FindObjectOfType<T>();
					if(_instance == null)
					{
						GameObject obj = new GameObject() { name = typeof(T).Name };
						_instance = obj.AddComponent<T>();
					}
				}
				return _instance;
			}
		}

		protected virtual void Awake()
		{
			if(_instance == null)
			{
				_instance = this as T;

				if(_dontDestroyOnLoad)
					DontDestroyOnLoad(this.gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}

		protected virtual void OnDestroy() => _instance = null;
	}
}