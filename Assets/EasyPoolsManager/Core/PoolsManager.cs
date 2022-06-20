using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace EasyPoolsManager
{
    public struct PoolData
    {
        /// <summary>
        /// The pool's identifier.
        /// </summary>
        public int Id { get; private set; }
        public string DisplayName { get; private set; }
        public ObjectPool<PoolableObject> Pool;

        public PoolData(int id, ObjectPool<PoolableObject> pool)
        {
            Id = id;
            DisplayName = $"Pool ({id})";
            Pool = pool;
        }

        public PoolData(int id, string displayName, ObjectPool<PoolableObject> pool)
        {
            Id = id;
            DisplayName = displayName;
            Pool = pool;
        }
    }

    /// <summary>
    /// Manages the pools of objects.
    /// </summary>
    public class PoolsManager : MonoBehaviour
    {
        #region Singleton
        private static PoolsManager instance;
        public static PoolsManager Instance {
            get {
                if (instance == null)
                    instance = FindObjectOfType<PoolsManager>();

                if (instance == null)
                    instance = new GameObject("Pools Manager").AddComponent<PoolsManager>();

                return instance;
            }
        }
        #endregion

        private List<PoolData> poolsData = new List<PoolData>();
        public List<PoolData> PoolsData => poolsData;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
        }

        /// <summary>
        /// Gets an object from a pool. If pool doesn't exist, both pool and object will be created.
        /// </summary>
        public PoolableObject GetFromPool(PrefabPool prefabPool)
        {
            ObjectPool<PoolableObject> pool = GetPool(prefabPool);
            PoolableObject obj = pool.Get();
            obj.SetReleaseAction(() => pool.Release(obj));

            return obj;
        }

        /// <summary>
        /// Get a pool, if pool doesn't exist then a new pool will be created.
        /// </summary>
        private ObjectPool<PoolableObject> GetPool(PrefabPool prefabPool)
        {
            // If pool doesn't exist then create it.
            if (!FindPool(prefabPool, out var foundPool))
                return CreatePool(prefabPool);

            // If pool exists then return it.
            return foundPool;
        }

        /// <summary>
        /// Creates a new pool in the scene and add it to the list of pools.
        /// </summary>
        private ObjectPool<PoolableObject> CreatePool(PrefabPool prefabPool)
        {
            System.Text.StringBuilder containerName = new System.Text.StringBuilder();
            containerName.Append("[Pool] ");
            containerName.Append(prefabPool.name);

            Transform container = new GameObject(containerName.ToString()).transform;
            ObjectPool<PoolableObject> newPool = prefabPool.CreatePool(container);

            poolsData.Add(new PoolData(prefabPool.GetHashCode(), prefabPool.name, newPool));
            return newPool;
        }

        /// <summary>
        /// Find a pool by id. Returns true if pool was found.
        /// </summary>
        public bool FindPool(int hashId, out ObjectPool<PoolableObject> pool)
        {
            for (int i = 0; i < poolsData.Count; i++)
            {
                if (poolsData[i].Id == hashId)
                {
                    pool = poolsData[i].Pool;
                    return true;
                }
            }

            pool = null;
            return false;
        }


        /// <summary>
        /// Find a pool. Returns true if pool was found.
        /// </summary>
        public bool FindPool(PrefabPool prefabPool, out ObjectPool<PoolableObject> pool)
        {
            int id = prefabPool.GetHashCode();
            return FindPool(id, out pool);
        }
    }
}
