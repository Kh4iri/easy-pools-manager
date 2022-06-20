using UnityEngine;

namespace EasyPoolsManager.Sample
{
    public class PoolableBullet : PoolableObject
    {
        [SerializeField] private PrefabPool impactFX;
        [SerializeField] private float speed = 50f;
        [SerializeField] private float lifetime = 5f;

        private float spawnedTimestamp;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            spawnedTimestamp = Time.time;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            Vector2 previousPosition = transform.position;
            transform.Translate(speed * Time.deltaTime, 0, 0);
            RaycastHit2D hit = Physics2D.Linecast(previousPosition, transform.position);

            if (hit)
            {
                OnHit(hit);
                return;
            }

            if (IsLifetimeExpired())
            {
                ReturnToPool();
                return;
            }
        }

        private void OnHit(RaycastHit2D hit)
        {
            impactFX.Get(hit.point, Quaternion.identity);
            ReturnToPool();
        }

        /// <summary>
        /// Called when the instance is taken from the pool. The base code is setting the game object to active.
        /// </summary>
        public override void OnGetFromPool(PoolableObject obj)
        {
            base.OnGetFromPool(obj);

            var bullet = obj as PoolableBullet;
            bullet.ResetLifetime();
        }

        private void ResetLifetime()
            => spawnedTimestamp = Time.time;

        private bool IsLifetimeExpired()
            => Time.time - spawnedTimestamp > lifetime;
    }
}
