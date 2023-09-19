using JT.GameEvents;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace JT.FGP
{
    /// <summary>
    /// Handle day and night cycle in game.
    /// </summary>
    public class DayNightCycleControl : MonoBehaviour, IDayCounter<int>
    {
        #region structs

        /// <summary>
        /// Each timeline in one
        /// </summary>
        [System.Serializable]
        public sealed class DayTimeDuration
        {
            #region Variables

            [SerializeField]
            private InGameTimeOfDay _onDayTime;

            [SerializeField]
            private AnimationCurve _lightIntensity;

            [SerializeField]
            private Color _lightColor;

            [SerializeField]
            private bool _cacheFixDuration = true;

            // Runtime variable data.
            private float _cacheDuration = 0f;
            private bool _isCached = false;

            #endregion

            #region Properties

            /// <summary>
            /// On this day time.
            /// </summary>
            public InGameTimeOfDay OnDayTime => _onDayTime;

            /// <summary>
            /// Light intensity on this day time.
            /// This also contains duration of day time.
            /// </summary>
            public AnimationCurve LightIntensity => _lightIntensity;

            /// <summary>
            /// The peak of light color when reaching this day time.
            /// The peak value can be reach on half duration as a transition between previous and upcoming day time.
            /// </summary>
            public Color LightColor => _lightColor;

            /// <summary>
            /// Duration of this day time.
            /// </summary>
            public float Duration
            {
                get
                {
                    if (_cacheFixDuration && _isCached) return _cacheDuration;
                    _cacheDuration = _lightIntensity.keys[_lightIntensity.keys.Length - 1].time;
                    _isCached = true;
                    return _cacheDuration;
                }
            }

            #endregion
        }

        #endregion

        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private Light2D _mainLight = null;

        [Header("Properties")]
        [SerializeField]
        private DayTimeDuration[] _dayTimeSequenceLoop = new DayTimeDuration[0];

        [SerializeField]
        private Timer _timer = new Timer();

        [Header("Game Events")]
        [SerializeField]
        private GameEventUnityObject _registerLightCallback = null;

        // Runtime variable data.
        private Dictionary<InGameLightingType, List<Light2DSwitchFunc>> _lights = new();
        private DayTimeDuration _tempDTD = null;
        private int _dayTimeCurrentIndex = 0;
        private int _dayCount = 0;
        private float _tempOnSecond = 0f;

        #endregion

        #region Mono

        private void Awake()
        {
            // Instantiate container if not yet init.
            if (_lights == null) _lights = new();
            if (_lights.Count == 0)
            {
                System.Array e = System.Enum.GetValues(typeof(InGameLightingType));
                InGameLightingType lightingType;
                for (int i = 0; i < e.Length; i++)
                {
                    lightingType = (InGameLightingType)e.GetValue(i);
                    _lights[lightingType] = new List<Light2DSwitchFunc>();
                }
            }

            // Subscribe events.
            _registerLightCallback.AddListener(ListenRegisterLightCallback);
        }

        private void OnDestroy()
        {
            // Unsubscribe events.
            _registerLightCallback.RemoveListener(ListenRegisterLightCallback);
        }

        private void Start()
        {
            // Check timeline starting point.
            float initialStart = _timer.InitialSecond;
            float tempDuration;
            while (initialStart > 0f)
            {
                _dayTimeCurrentIndex = 0;
                for (; _dayTimeCurrentIndex < _dayTimeSequenceLoop.Length; _dayTimeCurrentIndex++)
                {
                    tempDuration = _dayTimeSequenceLoop[_dayTimeCurrentIndex].Duration;
                    if (tempDuration > initialStart)
                        goto SkipDurationChecker;
                    initialStart -= tempDuration;
                }

                // Add day count.
                _dayCount++;
            }

            // Skip duration check if already valid.
            SkipDurationChecker:

            // Set timer start immediately.
            _timer.Start();
        }

        private void Update()
        {
            // Ticking timer.
            _timer.Tick(Time.deltaTime);

            // Transitioning day time values.
            _tempDTD = _dayTimeSequenceLoop[_dayTimeCurrentIndex];
            _tempOnSecond = _timer.OnSecond;
            _mainLight.intensity = _tempDTD.LightIntensity.Evaluate(_tempOnSecond);

            // Lerping light color.
            Color currentColor = _dayTimeSequenceLoop[_dayTimeCurrentIndex].LightColor;
            float halfDuration = _tempDTD.Duration / 2f;
            if (_tempOnSecond < halfDuration)
            {
                // Define previous index and previous color.
                int prevIndex = _dayTimeCurrentIndex - 1;
                prevIndex = prevIndex < 0 ? _dayTimeSequenceLoop.Length - 1 : prevIndex;
                Color prevColor = _dayTimeSequenceLoop[prevIndex].LightColor;

                // Calculate lerp color.
                _mainLight.color = Color.Lerp(prevColor, currentColor,
                    0.5f + (_tempOnSecond / halfDuration) / 2f);
            }
            else
            {
                // Define next index and next color.
                int nextIndex = _dayTimeCurrentIndex + 1;
                nextIndex = nextIndex >= _dayTimeSequenceLoop.Length ? 0 : nextIndex;
                Color nextColor = _dayTimeSequenceLoop[nextIndex].LightColor;

                // Calculate lerp color.
                _mainLight.color = Color.Lerp(currentColor, nextColor, _tempOnSecond / halfDuration / 2f);
            }

            // Check next sequence index of day time, if not the ignore the rest.
            if (_tempOnSecond < _tempDTD.Duration)
                return;

            // To the next sequence, and reset timer back to zero.
            _dayTimeCurrentIndex++;
            _timer.Reset(0f, false);

            // Check next day, if not exceeding day time sequence then ignore the rest.
            if (_dayTimeCurrentIndex < _dayTimeSequenceLoop.Length)
                return;

            // Next day.
            _dayTimeCurrentIndex = 0;
            _dayCount++;
        }

        #endregion

        #region IDayCounter

        /// <summary>
        /// Current day, starts from zero.
        /// </summary>
        public int DayCount => _dayCount;

        public void ResetDayCount() => _dayCount = 0;

        #endregion

        #region Main

        private void ListenRegisterLightCallback(Object lightObj)
        {
            // Check validation.
            if (lightObj is not Light2D) return;

            // Register light, manually check for better performance with valid registration.
            Light2DSwitchFunc light = (Light2DSwitchFunc)lightObj;
            if (InGameLightingType.Unknown == (light.LightType & InGameLightingType.Unknown))
                _lights[InGameLightingType.Unknown].Add(light);
            if (InGameLightingType.Global == (light.LightType & InGameLightingType.Global))
                _lights[InGameLightingType.Global].Add(light);
            if (InGameLightingType.Primary == (light.LightType & InGameLightingType.Primary))
                _lights[InGameLightingType.Primary].Add(light);
            if (InGameLightingType.Secondary == (light.LightType & InGameLightingType.Secondary))
                _lights[InGameLightingType.Secondary].Add(light);
            if (InGameLightingType.Tertiary == (light.LightType & InGameLightingType.Tertiary))
                _lights[InGameLightingType.Tertiary].Add(light);
            if (InGameLightingType.Quartenary == (light.LightType & InGameLightingType.Quartenary))
                _lights[InGameLightingType.Quartenary].Add(light);
            if (InGameLightingType.Quinary == (light.LightType & InGameLightingType.Quinary))
                _lights[InGameLightingType.Quinary].Add(light);
            if (InGameLightingType.Senary == (light.LightType & InGameLightingType.Senary))
                _lights[InGameLightingType.Senary].Add(light);
            if (InGameLightingType.Septenary == (light.LightType & InGameLightingType.Septenary))
                _lights[InGameLightingType.Septenary].Add(light);
            if (InGameLightingType.Octonary == (light.LightType & InGameLightingType.Octonary))
                _lights[InGameLightingType.Octonary].Add(light);
            if (InGameLightingType.Nonary == (light.LightType & InGameLightingType.Nonary))
                _lights[InGameLightingType.Nonary].Add(light);
            if (InGameLightingType.Denary == (light.LightType & InGameLightingType.Denary))
                _lights[InGameLightingType.Denary].Add(light);
        }

        #endregion
    }
}
