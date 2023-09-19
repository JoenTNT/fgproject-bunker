using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Store a preset for light switch.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Light Switch Preset",
        menuName = "FGP/Lighting/Light Switch Preset")]
    public class LightSwitchPresetSO : ScriptableObject
    {
        #region Variables

        [Header("Properties")]
        [SerializeField]
        private Light2DSwitchFunc.Meta _lightMeta = new Light2DSwitchFunc.Meta(
            AnimationCurve.Linear(0f, 0f, 1f, 1f), InGameLightingType.Unknown, false, false, false);

        #endregion

        #region Properties

        /// <summary>
        /// How will the light switch on and off transition gonna be.
        /// This will contains time and value, including min and max value on animation timeline.
        /// </summary>
        public AnimationCurve FadingCurve => _lightMeta.FadingCurve;

        /// <summary>
        /// Physical light type.
        /// </summary>
        public InGameLightingType LightType => _lightMeta.LightType;

        /// <summary>
        /// Turn off when the game started.
        /// </summary>
        public bool TurnOffOnStart => _lightMeta.TurnOffOnStart;

        /// <summary>
        /// Should the animation curve reversed when turning it off?
        /// </summary>
        public bool ReverseCurve => _lightMeta.ReverseCurve;

        /// <summary>
        /// Is the light turning on and off in loop?
        /// </summary>
        public bool Loop => _lightMeta.Loop;

        #endregion
    }
}

