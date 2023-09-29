using JT.GameEvents;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace JT.FGP
{
    /// <summary>
    /// Handle fading a target light.
    /// </summary>
    [RequireComponent(typeof(Light2D))]
    public class Light2DSwitchFunc : MonoBehaviour, IPhysicalSwitch, IRunnableCommand
    {
        #region structs

        /// <summary>
        /// Handle fading process using job system.
        /// </summary>
        public struct ExecutionJob : IJobParallelFor
        {
            // TODO: Animation curve data parsing problem.
            // This only takes 2 keyframe, which is start and end of animation curve.
            public readonly NativeArray<Keyframe> _firstKeyframe; 
            public readonly NativeArray<Keyframe> _secondKeyframe;
            public NativeArray<float> currentIntensityValue;
            public NativeArray<bool> isTurningOn; // Turning off if false.
            public float deltaTime;

            public void Execute(int index)
            {

            }
        }

        /// <summary>
        /// Lighting meta data.
        /// </summary>
        [System.Serializable]
        public struct Meta
        {
            #region Variables

            [SerializeField]
            private AnimationCurve _fadingCurve;

            [SerializeField]
            private InGameLightingType _lightType;

            [SerializeField]
            private bool _turnOffOnStart;

            [Tooltip("Fading animation curve running reverse from 1 to 0 instead of always 0 to 1")]
            [SerializeField]
            private bool _reverseCurve;

            [Tooltip("Always switch on and off.")]
            [SerializeField]
            private bool _loop;

            #endregion

            #region Properties

            /// <summary>
            /// How will the light switch on and off transition gonna be.
            /// This will contains time and value, including min and max value on animation timeline.
            /// </summary>
            public AnimationCurve FadingCurve => _fadingCurve;

            /// <summary>
            /// Physical light type.
            /// </summary>
            public InGameLightingType LightType
            {
                get => _lightType;
                set => _lightType = value;
            }

            /// <summary>
            /// Turn off when the game started.
            /// </summary>
            public bool TurnOffOnStart => _turnOffOnStart;

            /// <summary>
            /// Should the animation curve reversed when turning it off?
            /// </summary>
            public bool ReverseCurve => _reverseCurve;

            /// <summary>
            /// Is the light turning on and off in loop?
            /// </summary>
            public bool Loop
            {
                get => _loop;
                set => _loop = value;
            }

            #endregion

            #region Constructor

            public Meta(AnimationCurve fadingCurve, InGameLightingType lightType, bool turnOffOnStart,
                bool reverseCurve, bool loop)
            {
                _fadingCurve = fadingCurve;
                _lightType = lightType;
                _turnOffOnStart = turnOffOnStart;
                _reverseCurve = reverseCurve;
                _loop = loop;
            }

            #endregion
        }

        #endregion

        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private Light2D _targetLight = null;

        [Header("Properties")]
        [SerializeField]
        private Meta _lightMeta = new Meta(AnimationCurve.Linear(0f, 0f, 1f, 1f),
            InGameLightingType.Unknown, false, false, false);

        [Header("Optional")]
        [Tooltip("This will replace data when the object awakes, unset this to use current meta properties.")]
        [SerializeField]
        private LightSwitchPresetSO _preset = null;

        [Header("Optional Game Events")]
        [SerializeField]
        private GameEventUnityObject _sendToContainerCallback = null;

        // Runtime variable data.
        private float _currentIntensity = 0f;
        private bool _isTurnedOn = false;
        private bool _isTransitioning = false;

        #endregion

        #region Properties

        /// <summary>
        /// Physical light type.
        /// </summary>
        public InGameLightingType LightType
        {
            get => _lightMeta.LightType;
            set => _lightMeta.LightType = value;
        }

        /// <summary>
        /// Intensity of current light.
        /// </summary>
        public float LightIntensity
        {
            get => _targetLight.intensity;
            set => _targetLight.intensity = value;
        }

        /// <summary>
        /// State when light switch is turning on or off process.
        /// When the loop ticked, then this state will keep running.
        /// Turning it off the process using stop command.
        /// </summary>
        public bool IsTransitioning => _isTransitioning;

        #endregion

        #region Mono

        private void Awake()
        {
            // Check if using preset, then set data to meta immediately.
            if (_preset != null)
                _lightMeta = new Meta(_preset.FadingCurve, _preset.LightType, _preset.TurnOffOnStart,
                    _preset.ReverseCurve, _preset.Loop);

            // Initialize data.
            Init();
        }

        private void Start()
        {
            // Send it to lighting container to be controlled by manager.
            if (_sendToContainerCallback != null)
                _sendToContainerCallback.Invoke(this);
        }

        private void Update()
        {
            // Check running process, if not then ignore process.
            if (!_isTransitioning) return;

            // TODO: Switch transition process.
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            // Initalize only in editor, do not process in runtime.
            if (Application.isPlaying) return;
            Init();
        }
#endif
        #endregion

        #region IPhysicalSwitch

        public bool IsTurnedOn => _isTurnedOn;

        public void TurnOn() => _isTurnedOn = true;

        public void TurnOff() => _isTurnedOn = false;

        #endregion

        #region IRunnableCommand

        public void StartRun() => _isTransitioning = true;

        public void StopRun() => _isTransitioning = false;

        #endregion

        #region Main

        private void Init()
        {
            // Check target light is filled, if not then try find it.
            if (_targetLight == null)
                TryGetComponent(out _targetLight);

            // Check turn off on init.
            Keyframe firstKey = _lightMeta.FadingCurve.keys[0];
            Keyframe lastKey = _lightMeta.FadingCurve.keys[_lightMeta.FadingCurve.keys.Length - 1];
            _targetLight.intensity = _lightMeta.TurnOffOnStart ? firstKey.value : lastKey.value;
        }

        /// <summary>
        /// Shortcut for TurnOn and StartRun, only runnable when game is at runtime.
        /// </summary>
        [ContextMenu("[RunCommand] Run Light On")]
        public void RunLightOn()
        {
            // Check for game is running, if not then abort the process.
            if (!Application.isPlaying) return;

            TurnOn();
            StartRun();
        }

        /// <summary>
        /// Shortcut for TurnOff and StartRun, only runnable when game is at runtime.
        /// </summary>
        [ContextMenu("[RunCommand] Run Light Off")]
        public void RunLightOff()
        {
            // Check for game is running, if not then abort the process.
            if (!Application.isPlaying) return;

            TurnOff();
            StartRun();
        }

        // TODO: Implement Job System.
        public ExecutionJob CreateJob()
        {
            return new ExecutionJob();
        }

        #endregion
    }
}
