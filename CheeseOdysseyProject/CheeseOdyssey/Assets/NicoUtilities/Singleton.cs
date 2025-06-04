using UnityEngine;
namespace NicoUtilities
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;
        public static T Instance {
            get {
                if (_instance == null)
                    _instance = FindFirstObjectByType<T>();

                if (_instance == null)
                    Debug.LogError($"Singleton<{typeof(T)}> instance not found in scene!");

                return _instance;
            }
            private set {
                _instance = value;
            }
        }

        public static bool isInitialized => Instance != null;

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = (T)this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

}