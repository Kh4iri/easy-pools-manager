using System;
using UnityEngine;

// The class that derives from PoolableObject must invoke "ReturnToPool" method when it is no longer used.

namespace EasyPoolsManager
{
    /// <summary>
    /// Poolable game object.
    /// </summary>
    public abstract class PoolableObject : MonoBehaviour
    {
        private Action Release;

        /// <summary>
        /// Called from the pools manager to set the release action.
        /// </summary>
        public void SetReleaseAction(Action release)
            => Release = release;

        /// <summary>
        /// Called when the object is no longer used and will return to the pool.
        /// </summary>
        public void ReturnToPool()
            => Release?.Invoke();

        /// <summary>
        /// Called to create a new instance when the pool is empty. The base code is instantiating the prefab.
        /// </summary>
        /// <returns>The new instance.</returns>
        public virtual PoolableObject CreateNewInstance()
            => Instantiate(this);

        /// <summary>
        /// Called when the object is taken from the pool. The base code is setting the game object to active.
        /// </summary>
        public virtual void OnGetFromPool(PoolableObject obj)
            => obj.gameObject.SetActive(true);

        /// <summary>
        /// Called when the object is returned to the pool. The base code is setting the game object to inactive.
        /// </summary>
        public virtual void OnReleaseToPool(PoolableObject obj)
            => obj.gameObject.SetActive(false);
    }
}
