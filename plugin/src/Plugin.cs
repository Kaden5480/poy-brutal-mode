using BepInEx;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace BrutalModePlugin {
    [BepInPlugin("com.github.Kaden5480.poy-brutal-mode", "Brutal Mode", PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin {
        /**
         * <summary>
         * The new gravity value to use.
         * </summary>
         */
        private Vector3 gravity = new Vector3(0, -9.8f * 1.5f, 0);

        /**
         * <summary>
         * Executes when the plugin is being loaded.
         * </summary>
         */
        public void Awake() {
            SceneManager.sceneLoaded += this.OnSceneLoaded;
        }

        /**
         * <summary>
         * Executes when the plugin object is destroyed.
         * </summary>
         */
        public void OnDestroy() {
            SceneManager.sceneLoaded -= this.OnSceneLoaded;
        }

        /**
         * <summary>
         * Executes whenever a scene loads.
         * </summary>
         * <param name="scene">The scene which loaded</param>
         * <param name="mode">The mode the scene was loaded with</param>
         */
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            // Update gravity
            Physics.gravity = this.gravity;
        }
    }
}
