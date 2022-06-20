using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyPoolsManager
{
    /// <summary>
    /// Add this script to any scene to display debug information.
    /// </summary>
    public class PoolsDebugGUI : MonoBehaviour
    {
        private List<PoolData> poolsData = new List<PoolData>();

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            poolsData = PoolsManager.Instance.PoolsData;
        }

        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        void OnGUI()
        {
            DrawPoolsData(Vector2.one * 10f);
            DrawSceneButtons();
        }

        private void DrawPoolsData(Vector2 pos)
        {
            float labelWidth = 250f;
            float labelHeight = 20f;

            GUI.Label(new Rect(pos.x, pos.y, labelWidth, labelHeight * 2f),
                "<color=cyan>Easy Pools Manager</color>\n" +
                $"Active Pools: <color=lime>{poolsData.Count}</color>"
            );

            System.Text.StringBuilder poolsText = new System.Text.StringBuilder();

            for (int i = 0; i < poolsData.Count; i++)
            {
                var data = poolsData[i];
                var pool = data.Pool;

                // Format: ({Id}) {displayName}: {activeObjects} / {totalObjects}
                poolsText.AppendLine(
                    $"<color=grey>({data.Id})</color> {data.DisplayName}: <color=lime>{pool.CountActive} / {pool.CountAll}</color>"
                );
            }

            GUI.Label(new Rect(pos.x, pos.y + 40f, labelWidth, labelHeight * poolsData.Count), poolsText.ToString());
        }

        private void DrawSceneButtons()
        {
            Vector2 buttonSize = new Vector2(120f, 30f);

            Scene activeScene = SceneManager.GetActiveScene();
            bool previousSceneExists = activeScene.buildIndex > 0;
            bool nextSceneExists = activeScene.buildIndex < SceneManager.sceneCountInBuildSettings - 1;

            if (previousSceneExists)
            {
                // Draw "Previous Scene" button on bottom left of the screen.
                if (GUI.Button(new Rect(10f, Screen.height - buttonSize.y - 10f, buttonSize.x, buttonSize.y), "Previous Scene"))
                    SceneManager.LoadScene(activeScene.buildIndex - 1);
            }

            // Draw "Restart Scene" button on bottom middle of the screen.
            if (GUI.Button(new Rect(Screen.width / 2f - buttonSize.x / 2f, Screen.height - buttonSize.y - 10f, buttonSize.x, buttonSize.y), "Restart Scene"))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            if (nextSceneExists)
            {
                // Draw "Next Scene" button on bottom right of the screen.
                if (GUI.Button(new Rect(Screen.width - buttonSize.x - 10f, Screen.height - buttonSize.y - 10f, buttonSize.x, buttonSize.y), "Next Scene"))
                    SceneManager.LoadScene(activeScene.buildIndex + 1);
            }
        }
    }
}
