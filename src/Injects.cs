using UnityEngine;

namespace BrutalMode {
    /**
     * <summary>
     * Default values/options for the game.
     * </summary>
     */
    public static class Defaults {
        public static Vector3 gravity              { get; } = new Vector3(0, -9.8f, 0);

        // Crimps
        public static float crimpDrain             { get; } = 25f;
        public static float crimpDrainExtreme      { get; } = -50f;

        // Crumbling holds
        public static float crumblingHoldDecrement { get; } = 0.22f;

        // Pitches
        public static float pitchDrainChalk        { get; } = 0.75f;
        public static float pitchDrainNoChalk      { get; } = 1f;

        // Slopers
        public static float sloperDragA            { get; } = 75f;
        public static float sloperDragB            { get; } = 62f;
        public static float sloperDragC            { get; } = 35f;
        public static float sloperDragD            { get; } = 15f;
        public static float sloperDragE            { get; } = 5f;
        public static float sloperDragF            { get; } = 1.5f;

        // Ice picks
        public static float icePickDrain           { get; } = 3.5f;
        public static float icePickForceOne        { get; } = 8.2f;
        public static float icePickForceBoth       { get; } = 5.2f;
    }

    /**
     * <summary>
     * Values/methods which will be injected into the game.
     * </summary>
     */
    public static class Injects {
        // Increase gravity by 50%
        public static Vector3 gravity              = Defaults.gravity * 1.5f;

        // Crimp stamina drains 2x faster
        public static float crimpDrain             = Defaults.crimpDrain        * 2f;
        public static float crimpDrainExtreme      = Defaults.crimpDrainExtreme * 2f;

        // Pitch stamina drains 4x faster
        public static float pitchDrainChalk        = Defaults.pitchDrainChalk * 4f;
        public static float pitchDrainNoChalk      = Defaults.pitchDrainNoChalk * 4f;

        // Crumbling holds drain 3x faster
        public static float crumblingHoldDecrement = Defaults.crumblingHoldDecrement * 3f;

        // All higher value slopers have 8f drag
        private static float newSloperDrag         = 8f;
        public static float sloperDragA            = newSloperDrag;
        public static float sloperDragB            = newSloperDrag;
        public static float sloperDragC            = newSloperDrag;
        public static float sloperDragD            = newSloperDrag;
        public static float sloperDragE            = Defaults.sloperDragE;
        public static float sloperDragF            = Defaults.sloperDragF;

        // Ice pick stamina drains 4x faster
        public static float icePickDrain           = Defaults.icePickDrain * 4f;

        // Ice pick force is 10% stronger
        public static float icePickForceOne        = Defaults.icePickForceOne  * 1.1f;
        public static float icePickForceBoth       = Defaults.icePickForceBoth * 1.1f;
    }
}
