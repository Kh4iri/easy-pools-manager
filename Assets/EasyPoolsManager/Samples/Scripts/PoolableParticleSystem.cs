using UnityEngine;

namespace EasyPoolsManager.Sample
{
    /// <summary>
    /// This particle system will return to the pool when all particles have died.
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public class PoolableParticleSystem : PoolableObject
    {
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            var main = GetComponent<ParticleSystem>().main;

            if (main.stopAction != ParticleSystemStopAction.Callback)
            {
                Debug.LogWarning("[PoolableParticleSystem] stopAction is not set to Callback. Setting it to Callback.");
                main.stopAction = ParticleSystemStopAction.Callback;
            }
        }

        /// <summary>
        /// OnParticleSystemStopped is called when all particles in the system have died, and no new particles will be born.
        /// </summary>
        private void OnParticleSystemStopped()
        {
            ReturnToPool();
        }
    }
}
