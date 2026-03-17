// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using UnityEngine;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Basic singleton pattern for MonoBehaviours.
    /// Does NOT persist across scene loads. For persistent singletons, use PersistentSingleton.
    /// The Instance property will auto-create if none exists in the scene.
    /// </summary>
    /// <typeparam name="T">The MonoBehaviour-derived type.</typeparam>
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        protected static T instance;

        /// <summary>
        /// Returns true if an instance currently exists.
        /// </summary>
        public static bool HasInstance
        {
            get { return instance != null; }
        }

        /// <summary>
        /// Returns the instance if it exists, or null without auto-creating.
        /// </summary>
        /// <returns>The instance or null.</returns>
        public static T TryGetInstance()
        {
            return HasInstance ? instance : null;
        }

        /// <summary>
        /// Returns the singleton instance, finding or creating one if necessary.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindAnyObjectByType<T>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject(typeof(T).Name + " (Auto-Generated)");
                        instance = go.AddComponent<T>();
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Override Awake in subclasses but always call base.Awake().
        /// </summary>
        protected virtual void Awake()
        {
            InitializeSingleton();
        }

        protected virtual void InitializeSingleton()
        {
            if (!Application.isPlaying) return;

            instance = this as T;
        }
    }
}
