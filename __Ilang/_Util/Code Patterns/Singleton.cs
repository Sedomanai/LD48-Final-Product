using UnityEngine;

namespace Ilang {
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        static GameObject _container;
        static T _instance;
        
        public static T Instance {
            get {
                if (!_instance) {
                    _instance = FindObjectOfType<T>();
                    if (!_instance) {
                        _container = new GameObject();
                        _instance = _container.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }

        virtual protected void Awake() {
            if (this != Instance)
                Destroy(this);
            else DontDestroyOnLoad(gameObject);
        }

        virtual public void Init() {}

        public static void ResetManager() {
            if (_instance) {
                Destroy(_instance);
                _instance = _container.AddComponent<T>();
            }
        }

    }
}