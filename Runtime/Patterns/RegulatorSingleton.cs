// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using UnityEngine;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Persistent singleton that destroys any OLDER instances of the same type.
    /// Unlike PersistentSingleton (which destroys the newer duplicate),
    /// RegulatorSingleton keeps the newest instance and destroys older ones.
    /// Useful when hot-reloading or re-entering scenes that create a fresh instance.
    /// </summary>
    /// <typeparam name="T">The MonoBehaviour-derived type.</typeparam>
    public class RegulatorSingleton<T> : MonoBehaviour where T : Component
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
        /// The Time.time when this instance was initialized. Used to determine age.
        /// </summary>
        public float InitializationTime { get; private set; }

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
                        go.hideFlags = HideFlags.HideAndDontSave;
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

            InitializationTime = Time.time;
            DontDestroyOnLoad(gameObject);

            T[] existingInstances = FindObjectsByType<T>(FindObjectsSortMode.None);
            for (int i = 0; i < existingInstances.Length; i++)
            {
                RegulatorSingleton<T> regulator = existingInstances[i].GetComponent<RegulatorSingleton<T>>();
                if (regulator != null && regulator.InitializationTime < InitializationTime)
                {
                    Destroy(existingInstances[i].gameObject);
                }
            }

            if (instance == null)
            {
                instance = this as T;
            }
        }
    }
}
