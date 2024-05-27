using HarmonyLib;
using UnityEngine;


#if BEPINEX
using BepInEx;

using UnityEngine.SceneManagement;

namespace BrutalMode {
    [BepInPlugin("com.github.Kaden5480.poy-brutal-mode", "Brutal Mode", PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin {
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
            PatchGravity();
        }


#elif MELONLOADER
using MelonLoader;

[assembly: MelonInfo(typeof(BrutalMode.Plugin), "Brutal Mode", "0.2.0", "Kaden5480")]
[assembly: MelonGame("TraipseWare", "Peaks of Yore")]

namespace BrutalMode {
    public class Plugin: MelonMod {
        /**
         * <summary>
         * Executes whenever a scene loads.
         * </summary>
         * <param name="buildIndex">The build index of the scene</param>
         * <param name="sceneName">The name of the scene</param>
         */
        public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
            PatchGravity();
        }

#endif

        private void PatchGravity() {
            Physics.gravity = this.gravity;
        }
    }
}
