// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using UnityEngine;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Singleton that persists across scene loads via DontDestroyOnLoad.
    /// If a duplicate is found, the newer instance destroys itself.
    /// </summary>
    /// <typeparam name="T">The MonoBehaviour-derived type.</typeparam>
    public class PersistentSingleton<T> : MonoBehaviour where T : Component
    {
        /// <summary>
        /// When true, the singleton will unparent itself on Awake to ensure
        /// DontDestroyOnLoad works (root objects only).
        /// </summary>
        public bool AutoUnparentOnAwake = true;

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

            if (AutoUnparentOnAwake)
            {
                transform.SetParent(null);
            }

            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (instance != this)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
