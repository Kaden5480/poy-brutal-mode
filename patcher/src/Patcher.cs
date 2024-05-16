using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using MonoMod.Utils;

namespace HardModePatcher {
    public static class Patcher {
        /**
         * <summary>
         * The DLLs this patcher targets.
         * </summary>
         */
        public static IEnumerable<string> TargetDLLs { get; } = new string[] {
            "Assembly-CSharp.dll",
        };

        /**
         * <summary>
         * Patches the rate at which stamina drains on crimps.
         * </summary>
         * <param name="crimps">The type definition for crimps</param>
         */
        private static void PatchCrimpDrain(TypeDefinition crimps) {
            // Stamina decreases this many times faster
            const float multiplier = 2f;

            // Regular crimps
            MethodDefinition start = crimps.FindMethod("Start");
            Collection<Instruction> startInsts = start.Body.Instructions;

            string[][] seq = {
                new string[] { "ldarg.0" },
                new string[] { "ldc.r4", "25" },
                new string[] { "stfld", "System.Single MicroHolds::gripDecrementValue" },
            };

            int index = Helper.FindSeq(startInsts, seq);
            startInsts[index + 1].Operand = 25f * multiplier;

            // Extreme crimps
            MethodDefinition update = crimps.FindMethod("Update");
            Collection<Instruction> updateInsts = update.Body.Instructions;

            seq = new string[][] {
                new string[] { "ldarg.0" },
                new string[] { "ldc.r4", "-50" },
                new string[] { "stfld", "System.Single MicroHolds::gripDecrementExtremeValue" },
            };

            index = Helper.FindSeq(updateInsts, seq);
            updateInsts[index + 1].Operand = -50f * multiplier;
        }

        /**
         * <summary>
         * Patches the rate at which crumbling holds disappear.
         * </summary>
         * <param name="crumbling">The type definition for crumbling holds</param>
         */
        private static void PatchCrumblingHoldDrain(TypeDefinition crumbling) {
            // How much faster crumbling holds should disappear
            const float multiplier = 3f;

            MethodDefinition ctor = crumbling.FindMethod(".ctor");
            Collection<Instruction> insts = ctor.Body.Instructions;

            // Drain crumbling holds faster
            string[][] seq = {
                new string[] { "ldarg.0" },
                new string[] { "ldc.r4", "0.22" },
                new string[] { "stfld", "System.Single CrumblingHoldRegular::decreaseIncrement" },
            };

            int index = Helper.FindSeq(insts, seq);
            insts[index + 1].Operand = 0.22f * multiplier;
        }

        /**
         * <summary>
         * Patches the rate at which stamina drains on climbing pitches.
         * </summary>
         * <param name="pitches">The type definition for climbing pitches</param>
         */
        private static void PatchPitchDrain(TypeDefinition pitches) {
            // Stamina decreases this many times faster
            const float multiplier = 4f;

            MethodDefinition update = pitches.FindMethod("Update");
            Collection<Instruction> insts = update.Body.Instructions;

            // Left hand
            string[][] seq = {
                new string[] { "ldarg.0" },
                new string[] { "ldc.r4", "0.75" },
                new string[] { "stfld", "System.Single ClimbingPitches::drainAmountL" },
                new string[] { "br.s", null },
                new string[] { "ldarg.0" },
                new string[] { "ldc.r4", "1" },
                new string[] { "stfld", "System.Single ClimbingPitches::drainAmountL" },
            };

            int index = Helper.FindSeq(insts, seq);
            insts[index + 1].Operand = 0.75f * multiplier;
            insts[index + 5].Operand = 1f    * multiplier;

            // Right hand
            seq = new string[][] {
                new string[] { "ldarg.0" },
                new string[] { "ldc.r4", "0.75" },
                new string[] { "stfld", "System.Single ClimbingPitches::drainAmountR" },
                new string[] { "br.s", null },
                new string[] { "ldarg.0" },
                new string[] { "ldc.r4", "1" },
                new string[] { "stfld", "System.Single ClimbingPitches::drainAmountR" },
            };

            index = Helper.FindSeq(insts, seq);
            insts[index + 1].Operand = 0.75f * multiplier;
            insts[index + 5].Operand = 1f    * multiplier;

            // Both hands on the same hold
            seq = new string[][] {
                new string[] { "ldc.r4", "1" },
                new string[] { "stloc.0" },
                new string[] { "ldarg.0" },
                new string[] { "ldfld", "Climbing ClimbingPitches::climbing" },
                new string[] { "ldfld", "System.Boolean Climbing::grabbingPinchHoldLeft" },
                new string[] { "brfalse.s", null },
                new string[] { "ldarg.0" },
                new string[] { "ldfld", "Climbing ClimbingPitches::climbing" },
                new string[] { "ldfld", "System.Boolean Climbing::grabbingPinchHoldRight" },
                new string[] { "brfalse.s", null },
                new string[] { "ldc.r4", "2.5" },
                new string[] { "stloc.0" },
                new string[] { "br.s", null },
                new string[] { "ldc.r4", "1" },
                new string[] { "stloc.0" },
            };

            index = Helper.FindSeq(insts, seq);
            insts[index + 10].Operand = 2.5f * multiplier;
            insts[index + 13].Operand = 1f   * multiplier;
        }

        /**
         * <summary>
         * Patches the rate at which the player slides down sloper holds.
         * </summary>
         * <param name="slopers">The type definition for sloper holds</param>
         */
        private static void PatchSloperDrag(TypeDefinition slopers) {
            // Patches slopers to use this drag value
            // Doesn't patch slopers with drag values of 5 and 1.5
            const float drag = 8f;

            // Initial values:
            // [index +   2] = 75f
            // [index +  15] = 62f
            // [index +  28] = 35f
            // [index +  41] = 15f
            // [index +  54] = 5f
            // [index + 102] = 1.5f

            MethodDefinition update = slopers.FindMethod("Update");
            Collection<Instruction> insts = update.Body.Instructions;

            // stemPointRbL
            string[][] seq = {
                new string[] { "ldarg.0" },
                new string[] { "ldfld", "UnityEngine.Rigidbody SloperHold::stemPointRbL" },
                new string[] { "ldc.r4", "75" },
                new string[] { "callvirt", "System.Void UnityEngine.Rigidbody::set_drag(System.Single)" },
            };

            int index = Helper.FindSeq(insts, seq);
            insts[index +  2].Operand = drag;
            insts[index + 15].Operand = drag;
            insts[index + 28].Operand = drag;
            insts[index + 41].Operand = drag;

            // stemPointRbR
            seq = new string[][] {
                new string[] { "ldarg.0" },
                new string[] { "ldfld", "UnityEngine.Rigidbody SloperHold::stemPointRbR" },
                new string[] { "ldc.r4", "75" },
                new string[] { "callvirt", "System.Void UnityEngine.Rigidbody::set_drag(System.Single)" },
            };

            index = Helper.FindSeq(insts, seq);
            insts[index +  2].Operand = drag;
            insts[index + 15].Operand = drag;
            insts[index + 28].Operand = drag;
            insts[index + 41].Operand = drag;
        }

        /**
         * <summary>
         * Patches the rate at which stamina drains for ice picks.
         * </summary>
         * <param name="picks">The type definition for ice picks</param>
         */
        private static void PatchIcePickDrain(TypeDefinition picks) {
            // Stamina decreases this many times faster
            const float multiplier = 4f;

            MethodDefinition ctor = picks.FindMethod(".ctor");
            Collection<Instruction> insts = ctor.Body.Instructions;

            string[][] seq = {
                new string[] { "ldarg.0" },
                new string[] { "ldc.r4", "3.5" },
                new string[] { "stfld", "System.Single IceAxe::gripDecrementValue" },
            };

            int index = Helper.FindSeq(insts, seq);
            insts[index + 1].Operand = 3.5f * multiplier;
        }

        /**
         * <summary>
         * Patches the ice pick climbing force.
         * </summary>
         * <param name="picks">The type definition for ice picks</param>
         */
        private static void PatchIcePickForce(TypeDefinition picks) {
            // Increase ice pick force this many times
            const float multiplier = 1.1f;

            // Increase ice pick force
            MethodDefinition fixedUpdate = picks.FindMethod("FixedUpdate");
            Collection<Instruction> insts = fixedUpdate.Body.Instructions;

            string[][] seq = {
                new string[] { "ldarg.0" },
                new string[] { "ldc.r4", "5.2" },
                new string[] { "stfld", "System.Single IceAxe::flexForce" },
                new string[] { "br.s", null },
                new string[] { "ldarg.0" },
                new string[] { "ldc.r4", "8.2" },
                new string[] { "stfld", "System.Single IceAxe::flexForce" },
            };

            int index = Helper.FindSeq(insts, seq);
            insts[index + 1].Operand = 5.2f * multiplier;
            insts[index + 5].Operand = 8.2f * multiplier;
        }

        /**
         * <summary>
         * Patches the game.
         * </summary>
         * <param name="assembly">The assembly to patch</param>
         */
        public static void Patch(AssemblyDefinition assembly) {
            ModuleDefinition main = assembly.MainModule;

            // Patching holds
            PatchCrimpDrain(main.GetType("MicroHolds"));
            PatchCrumblingHoldDrain(main.GetType("CrumblingHoldRegular"));
            PatchPitchDrain(main.GetType("ClimbingPitches"));
            PatchSloperDrag(main.GetType("SloperHold"));

            // Patching tools
            PatchIcePickDrain(main.GetType("IceAxe"));
            PatchIcePickForce(main.GetType("IceAxe"));
        }
    }
}
