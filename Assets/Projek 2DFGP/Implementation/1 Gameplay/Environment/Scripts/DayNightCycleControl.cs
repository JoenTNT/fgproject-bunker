using JT.GameEvents;
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
            /// WARNING: This will read the last key of curve, cache this data for better performance.
            /// </summary>
            public float Duration => _lightIntensity.keys[_lightIntensity.keys.Length - 1].time;

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
        private GameEventFloat _onTickDayPercentage = null;

        // Runtime variable data.
        private DayTimeDuration _tempDTD = null;
        private int _dayTimeCurrentIndex = 0;
        private int _dayCount = 0;
        private float _tempOnSecond = 0f;
        private float _tempOnSecondToday = 0f;

        // Cache variable data.
        private float[] _cacheDurations = null;
        private float _secondsInOneDayCache = 0f;

        #endregion

        #region Properties

        /// <summary>
        /// How much seconds in real life for one day in gameplay.
        /// </summary>
        public float SecondsInOneDay => _secondsInOneDayCache;

        #endregion

        #region Mono

        private void Awake()
        {
            // Cache all data requirements.
            int seqCount = _dayTimeSequenceLoop.Length;
            _cacheDurations = new float[seqCount];
            _secondsInOneDayCache = 0f;
            for (int i = 0; i < seqCount; i++)
            {
                _cacheDurations[i] = _dayTimeSequenceLoop[i].Duration;
                _secondsInOneDayCache += _cacheDurations[i];
            }
        }

        private void Start()
        {
            // Check timeline starting point.
            float initialStart = _timer.InitialSecond;
            float tempDuration = 0f;
            while (initialStart > 0f)
            {
                _dayTimeCurrentIndex = 0;
                for (; _dayTimeCurrentIndex < _dayTimeSequenceLoop.Length; _dayTimeCurrentIndex++)
                {
                    tempDuration = _dayTimeSequenceLoop[_dayTimeCurrentIndex].Duration;
                    if (tempDuration > initialStart) goto SkipDurationChecker;
                    initialStart -= tempDuration;
                }

                // Add day count.
                _dayCount++;
            }

        // Skip duration check if already valid.
        SkipDurationChecker:

            // Set initial temp seconds today.
            _tempOnSecondToday = tempDuration;
            for (int i = 0; i < _dayTimeCurrentIndex; i++)
                _tempOnSecondToday += _cacheDurations[i];

            // Set timer start immediately.
            _timer.Start();
        }

        private void Update()
        {
            // Always call event before start everything.
            _onTickDayPercentage.Invoke(_tempOnSecondToday / _secondsInOneDayCache);

            // Ticking timer.
            float delta = Time.deltaTime;
            _timer.Tick(delta);
#if UNITY_EDITOR
            _tempOnSecondToday = _tempOnSecond;
            for (int i = 0; i < _dayTimeCurrentIndex; i++)
                _tempOnSecondToday += _cacheDurations[i];
#else
            _tempOnSecondToday += delta;
#endif
                // Transitioning day time values.
            _tempDTD = _dayTimeSequenceLoop[_dayTimeCurrentIndex];
            _tempOnSecond = _timer.OnSecond;
            _mainLight.intensity = _tempDTD.LightIntensity.Evaluate(_tempOnSecond);

            // Lerping light color.
            Color currentColor = _dayTimeSequenceLoop[_dayTimeCurrentIndex].LightColor;
            float halfDuration = _cacheDurations[_dayTimeCurrentIndex] / 2f;
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
                _mainLight.color = Color.Lerp(currentColor, nextColor,
                    (_tempOnSecond - halfDuration) / _cacheDurations[_dayTimeCurrentIndex]);
            }

            // Check next sequence index of day time, if not the ignore the rest.
            if (_tempOnSecond < _cacheDurations[_dayTimeCurrentIndex])
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
            _tempOnSecondToday = 0f;
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            // Cache new data everytime changed in editor.
            int seqCount = _dayTimeSequenceLoop.Length;
            _cacheDurations = new float[seqCount];
            _secondsInOneDayCache = 0f;
            for (int i = 0; i < seqCount; i++)
            {
                _cacheDurations[i] = _dayTimeSequenceLoop[i].Duration;
                _secondsInOneDayCache += _cacheDurations[i];
            }
        }
#endif
#endregion

        #region IDayCounter

        /// <summary>
        /// Current day, starts from zero.
        /// </summary>
        public int DayCount => _dayCount;

        public void ResetDayCount() => _dayCount = 0;

        #endregion
    }
}
