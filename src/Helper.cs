using System.Collections.Generic;

using HarmonyLib;

namespace BrutalMode {
    public class Helper {
        /**
         * <summary>
         * Compare two instructions for equivalence.
         * </summary>
         * <param name="a">The first instruction to compare</param>
         * <param name="b">The second instruction to compare</param>
         */
        public static bool InstsEqual(CodeInstruction a, CodeInstruction b) {
            // Check opcodes
            if (a.opcode != b.opcode) {
                return false;
            }

            // Check null operands
            if (a.operand == null || b.operand == null) {
                return a.operand == b.operand;
            }

            // Check operand equivalence
            return a.operand.Equals(b.operand);
        }

        /**
         * <summary>
         * Given a sequence of instructions, find a pattern and replace
         * it with the provided sequence.
         * </summary>
         * <param name="instructions">The instructions to search in</param>
         * <param name="pattern">The pattern to search for</param>
         * <param name="replacement">What to replace the pattern with</param>
         */
        public static IEnumerable<CodeInstruction> Replace(
            IEnumerable<CodeInstruction> instructions,
            CodeInstruction[] pattern,
            CodeInstruction[] replacement
        ) {
            List<CodeInstruction> buffer = new List<CodeInstruction>();
            int patternIndex = 0;

            // Handle empty patterns
            if (pattern.Length < 1) {
                foreach (var instruction in instructions) {
                    yield return instruction;
                }

                yield break;
            }

            foreach (var instruction in instructions) {
                // Full pattern matched, return replacement followed
                // by the current instruction
                if (patternIndex >= pattern.Length) {
                    foreach (var replace in replacement) {
                        yield return replace;
                    }

                    yield return instruction;

                    patternIndex = 0;
                    buffer.Clear();

                    continue;
                }

                // Instructions aren't the same, return anything in the buffer
                // followed by the current instruction
                if (InstsEqual(instruction, pattern[patternIndex]) == false) {
                    foreach (var buffered in buffer) {
                        yield return buffered;
                    }

                    yield return instruction;

                    patternIndex = 0;
                    buffer.Clear();

                    continue;
                }

                // Otherwise, add the instruction to the buffer
                buffer.Add(instruction);
                patternIndex++;
            }
        }
    }
}
