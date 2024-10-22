using UnityEngine;

namespace TFBG
{
    /// <summary>
    /// Base class for any object that should exist in the global scope.
    /// </summary>
    /// <remarks>
    /// <para>Access a singleton with its <see cref="Instance"/> static field.</para>
    /// <para>Should you need to check access to a singleton before using it, prefer using
    /// <see cref="TryGetInstance"/>, as it is cheaper than the combination of checking <see cref="InstanceExists"/>
    /// and then retrieving the <see cref="Instance"/>.</para>
    /// </remarks>
    /// <typeparam name="T">Type of the singleton</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        /// <summary>
        /// The static reference to the instance
        /// </summary>
        public static T Instance { get; protected set; }

        /// <summary>
        /// Gets whether an instance of this singleton exists
        /// </summary>
        public static bool InstanceExists { get { return Instance != null; } }


        /// <summary>
        /// Gets the instance of this singleton, and returns true if it is not null.
        /// Prefer this whenever you would otherwise use InstanceExists and Instance together.
        /// </summary>
        public static bool TryGetInstance(out T result)
        {
            result = Instance;

            return result != null;
        }


        /// <summary>
        /// Associates this singleton with instance
        /// </summary>
        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarningFormat("Trying to create a second instance of {0}", typeof(T));
                Destroy(gameObject);
            }
            else
            {
                Instance = (T)this;
            }
        }

        /// <summary>
        /// OnDestroy method to clear singleton association
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}