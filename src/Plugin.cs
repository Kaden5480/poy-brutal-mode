using System.Collections.Generic;

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

            Harmony.CreateAndPatchAll(typeof(PatchCrimpDrainRegular));
            Harmony.CreateAndPatchAll(typeof(PatchCrimpDrainExtreme));
            Harmony.CreateAndPatchAll(typeof(PatchCrumblingHoldDrain));
            Harmony.CreateAndPatchAll(typeof(PatchSloperDrag));
            Harmony.CreateAndPatchAll(typeof(PatchIcePickDrain));
            Harmony.CreateAndPatchAll(typeof(PatchIcePickForce));
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

        /**
         * <summary>
         * Patches gravity.
         * </summary>
         */
        private void PatchGravity() {
            Physics.gravity = Injects.gravity;
        }

        /**
         * <summary>
         * Patches regular crimp stamina drain.
         * </summary>
         */
        [HarmonyPatch(typeof(MicroHolds), "Start")]
        static class PatchCrimpDrainRegular {
            static IEnumerable<CodeInstruction> Transpiler(
                IEnumerable<CodeInstruction> insts
            ) {
                FieldInfo gripDecrementValue = AccessTools.Field(
                    typeof(MicroHolds), "gripDecrementValue"
                );

                IEnumerable<CodeInstruction> replaced = Helper.Replace(insts,
                    new[] {
                        new CodeInstruction(OpCodes.Ldc_R4, Defaults.crimpDrain),
                        new CodeInstruction(OpCodes.Stfld, gripDecrementValue),
                    },
                    new[] {
                        new CodeInstruction(OpCodes.Ldc_R4, Injects.crimpDrain),
                        new CodeInstruction(OpCodes.Stfld, gripDecrementValue),
                    }
                );

                foreach (CodeInstruction replace in replaced) {
                    yield return replace;
                }
            }
        }

        /**
         * <summary>
         * Patches extreme crimp stamina drain.
         * </summary>
         */
        [HarmonyPatch(typeof(MicroHolds), "Update")]
        static class PatchCrimpDrainExtreme {
            static IEnumerable<CodeInstruction> Transpiler(
                IEnumerable<CodeInstruction> insts
            ) {
                FieldInfo gripDecrementExtremeValue = AccessTools.Field(
                    typeof(MicroHolds), "gripDecrementExtremeValue"
                );

                IEnumerable<CodeInstruction> replaced = Helper.Replace(insts,
                    new[] {
                        new CodeInstruction(OpCodes.Ldc_R4, Defaults.crimpDrainExtreme),
                        new CodeInstruction(OpCodes.Stfld, gripDecrementExtremeValue),
                    },
                    new[] {
                        new CodeInstruction(OpCodes.Ldc_R4, Injects.crimpDrainExtreme),
                        new CodeInstruction(OpCodes.Stfld, gripDecrementExtremeValue),
                    }
                );

                foreach (CodeInstruction replace in replaced) {
                    yield return replace;
                }
            }
        }

        /**
         * <summary>
         * Patches the rate crumbling holds disappear.
         * </summary>
         */
        [HarmonyPatch(typeof(CrumblingHoldRegular))]
        [HarmonyPatch(MethodType.Constructor)]
        static class PatchCrumblingHoldDrain {
            static IEnumerable<CodeInstruction> Transpiler(
                IEnumerable<CodeInstruction> insts
            ) {
                FieldInfo decreaseIncrement = AccessTools.Field(
                    typeof(CrumblingHoldRegular), "decreaseIncrement"
                );

                IEnumerable<CodeInstruction> replaced = Helper.Replace(insts,
                    new[] {
                        new CodeInstruction(OpCodes.Ldc_R4, Defaults.crumblingHoldDecrement),
                        new CodeInstruction(OpCodes.Stfld, decreaseIncrement),
                    },
                    new[] {
                        new CodeInstruction(OpCodes.Ldc_R4, Injects.crumblingHoldDecrement),
                        new CodeInstruction(OpCodes.Stfld, decreaseIncrement),
                    }
                );

                foreach (CodeInstruction replace in replaced) {
                    yield return replace;
                }
            }
        }

        /**
         * <summary>
         * Patches drag value for a variety of slopers.
         * </summary>
         */
        [HarmonyPatch(typeof(SloperHold), "Update")]
        static class PatchSloperDrag {
            static IEnumerable<CodeInstruction> Transpiler(
                IEnumerable<CodeInstruction> insts
            ) {
                MethodInfo setDrag = AccessTools.Method(
                    typeof(Rigidbody), "set_drag"
                );

                IEnumerable<CodeInstruction> replaced = insts;

                // Patch all slopers
                foreach (char c in "ABCDEF") {
                    float def = (float) typeof(Defaults).GetProperty($"sloperDrag{c}")
                        .GetValue(null);

                    float inject = (float) typeof(Injects).GetField($"sloperDrag{c}")
                        .GetValue(null);

                    replaced = Helper.Replace(replaced,
                        new[] {
                            new CodeInstruction(OpCodes.Ldc_R4, def),
                            new CodeInstruction(OpCodes.Callvirt, setDrag),
                        },
                        new[] {
                            new CodeInstruction(OpCodes.Ldc_R4, inject),
                            new CodeInstruction(OpCodes.Callvirt, setDrag),
                        }
                    );
                }

                foreach (CodeInstruction replace in replaced) {
                    yield return replace;
                }
            }
        }

        /**
         * <summary>
         * Patches ice pick stamina drain.
         * </summary>
         */
        [HarmonyPatch(typeof(IceAxe))]
        [HarmonyPatch(MethodType.Constructor)]
        static class PatchIcePickDrain {
            static IEnumerable<CodeInstruction> Transpiler(
                IEnumerable<CodeInstruction> insts
            ) {
                FieldInfo gripDecrementValue = AccessTools.Field(
                    typeof(IceAxe), "gripDecrementValue"
                );

                IEnumerable<CodeInstruction> replaced = Helper.Replace(insts,
                    new[] {
                        new CodeInstruction(OpCodes.Ldc_R4, Defaults.icePickDrain),
                        new CodeInstruction(OpCodes.Stfld, gripDecrementValue),
                    },
                    new[] {
                        new CodeInstruction(OpCodes.Ldc_R4, Injects.icePickDrain),
                        new CodeInstruction(OpCodes.Stfld, gripDecrementValue),
                    }
                );

                foreach (CodeInstruction replace in replaced) {
                    yield return replace;
                }
            }
        }

        /**
         * <summary>
         * Patches ice pick climbing force.
         * </summary>
         */
        [HarmonyPatch(typeof(IceAxe), "FixedUpdate")]
        static class PatchIcePickForce {
            static IEnumerable<CodeInstruction> Transpiler(
                IEnumerable<CodeInstruction> insts
            ) {
                FieldInfo flexForce = AccessTools.Field(
                    typeof(IceAxe), "flexForce"
                );

                // Flex force with one hand
                IEnumerable<CodeInstruction> replaced = Helper.Replace(insts,
                    new[] {
                        new CodeInstruction(OpCodes.Ldc_R4, Defaults.icePickForceOne),
                        new CodeInstruction(OpCodes.Stfld, flexForce),
                    },
                    new[] {
                        new CodeInstruction(OpCodes.Ldc_R4, Injects.icePickForceOne),
                        new CodeInstruction(OpCodes.Stfld, flexForce),
                    }
                );

                // Flex force with both hands
                replaced = Helper.Replace(replaced,
                    new[] {
                        new CodeInstruction(OpCodes.Ldc_R4, Defaults.icePickForceBoth),
                        new CodeInstruction(OpCodes.Stfld, flexForce),
                    },
                    new[] {
                        new CodeInstruction(OpCodes.Ldc_R4, Injects.icePickForceBoth),
                        new CodeInstruction(OpCodes.Stfld, flexForce),
                    }
                );

                foreach (CodeInstruction replace in replaced) {
                    yield return replace;
                }
            }
        }
    }
}
