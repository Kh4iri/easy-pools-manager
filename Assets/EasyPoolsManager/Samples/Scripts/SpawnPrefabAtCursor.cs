using UnityEngine;

namespace EasyPoolsManager.Sample
{
    public class SpawnPrefabAtCursor : MonoBehaviour
    {
        [SerializeField] private new Camera camera;
        [SerializeField] private PrefabPool prefabPool1;
        [SerializeField] private PrefabPool prefabPool2;
        [SerializeField] private PrefabPool prefabPool3;

        [Space(10)]
        [Tooltip("Allow the mouse to be held down to spawn multiple objects?")]
        [SerializeField] private bool holdToSpawn = false;
        [Tooltip("The cooldown between each spawn. Only works if holdToSpawn is true.")]
        [SerializeField] private float spawnCooldown = 1f;

        private float spawnedTimestamp;

        /// <summary>
        /// Reset is called when the user hits the Reset button in the Inspector's
        /// context menu or when adding the component the first time.
        /// </summary>
        void Reset()
            => camera = Camera.main;

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            if (!camera)
                return;

            Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);

            if (!holdToSpawn)
            {
                if (Input.GetMouseButtonDown(0))
                    prefabPool1.Get(mousePos, Quaternion.identity);

                if (Input.GetMouseButtonDown(1))
                    prefabPool2?.Get(mousePos, Quaternion.identity);

                if (Input.GetMouseButtonDown(2))
                    prefabPool3?.Get(mousePos, Quaternion.identity);
            }
            else if (Time.time - spawnedTimestamp > spawnCooldown)
            {
                if (Input.GetMouseButton(0))
                    prefabPool1.Get(mousePos, Quaternion.identity);

                if (Input.GetMouseButton(1))
                    prefabPool2?.Get(mousePos, Quaternion.identity);

                if (Input.GetMouseButton(2))
                    prefabPool3?.Get(mousePos, Quaternion.identity);

                spawnedTimestamp = Time.time;
            }
        }
    }
}
