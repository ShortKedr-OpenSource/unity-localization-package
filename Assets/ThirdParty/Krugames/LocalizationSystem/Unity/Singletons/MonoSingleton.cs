using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Krugames.LocalizationSystem.Unity.Singletons {
    /// <summary>
    /// Realization of MonoBehaviour but as singleton unit.
    /// Presented realization don't mean "only-one" rule.
    /// Instance will be always presented as one instance,
    /// but you still can create other instances via UnityEngine system.
    /// Instance will be create after first call.
    /// </summary>
    /// <typeparam name="TEndType">type of inheritor</typeparam>
    public abstract class MonoSingleton<TEndType> : MonoBehaviour where TEndType : MonoSingleton<TEndType> {
        protected static TEndType _instance = null;

        public static TEndType Instance {
            get {
                if (_instance == null) {
#if UNITY_EDITOR
                    if (!EditorApplication.isPlaying) return _instance;
#endif
                    GameObject obj = new GameObject(typeof(TEndType).Name, typeof(TEndType));
                    DontDestroyOnLoad(obj);
                    _instance = obj.GetComponent<TEndType>();
                    _instance.InstanceCreate();
                    OnInstanceCreate?.Invoke();
                }
                return _instance;
            }
        }

        #region EVENTS
        public static event Action OnInstanceCreate;
        #endregion

        protected virtual void InstanceCreate() {
            OnInstanceCreate?.Invoke();
        }
    }
}