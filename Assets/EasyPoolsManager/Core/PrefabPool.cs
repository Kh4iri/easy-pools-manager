using UnityEngine;
using UnityEngine.Pool;

namespace EasyPoolsManager
{
    /// <summary>
    /// Prefab pool configuration.
    /// </summary>
    [CreateAssetMenu(fileName = "New Prefab Pool")]
    public class PrefabPool : ScriptableObject
    {
        [Header("Pool")]
        [Tooltip("Collection checks are performed when an instance is returned back to the pool. " +
            "An exception will be thrown if the instance is already in the pool. " +
            "Collection checks are only performed in the Editor.")]
        [SerializeField] private bool collectionCheck;

        [Tooltip("The default capacity the stack will be created with.")]
        [SerializeField, Min(5)] private int defaultCapacity = 5;

        [Tooltip("The maximum size of the pool. When the pool reaches the max size then " +
            "any further instances returned to the pool will be ignored and can be garbage collected. " +
            "This can be used to prevent the pool growing to a very large size.")]
        [SerializeField, Min(10)] private int maxSize = 15;

        [Header("Prefab")]
        [SerializeReference] private PoolableObject poolablePrefab;

        /// <summary>
        /// Get an object from the pool. If the pool is empty then a new instance will be created.<br />
        /// If the pool doesn't exist then it will be created.
        /// </summary>
        public PoolableObject Get()
        {
            var obj = PoolsManager.Instance.GetFromPool(this);
            return obj;
        }

        /// <summary>
        /// Get an object from the pool. If the pool is empty then a new instance will be created.
        /// </summary>
        /// <param name="position">The position of the object.</param>
        /// <param name="rotation">The rotation of the object.</param>
        public PoolableObject Get(Vector3 position, Quaternion rotation)
        {
            var obj = Get();
            obj.transform.SetPositionAndRotation(position, rotation);
            return obj;
        }

        /// <summary>
        /// Creates the pool, called from the pools manager.
        /// </summary>
        public ObjectPool<PoolableObject> CreatePool(Transform container = null)
        {
            return new ObjectPool<PoolableObject>(
                createFunc: () => {
                    var obj = poolablePrefab.CreateNewInstance();
                    obj.transform.parent = container;
                    return obj;
                },
                actionOnGet: poolablePrefab.OnGetFromPool,
                actionOnRelease: poolablePrefab.OnReleaseToPool,
                actionOnDestroy: (p) => Destroy(p.gameObject),
                collectionCheck: collectionCheck,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize
            );
        }
    }
}
