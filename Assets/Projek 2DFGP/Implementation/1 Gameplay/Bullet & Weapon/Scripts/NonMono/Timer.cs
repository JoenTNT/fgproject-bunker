using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Running timer handler in seconds.
    /// </summary>
    [System.Serializable]
    public sealed class Timer
    {
        #region events

        /// <summary>
        /// Event called when time is up.
        /// </summary>
        public event System.Action OnTimesUp;

        #endregion

        #region Variables

        [Header("Timer Properties")]
        [SerializeField]
        private float _initialTime = 3f;

        [SerializeField]
        private float _multiplier = 1f;

        [SerializeField]
        private float _timesUpOn = 0f;

        [SerializeField]
        private bool _isCountingUp = false;

        // Runtime variable data.
        private float _tempSeconds = 0f;
        private bool _isRunning = false;

        #endregion

        #region Properties

        /// <summary>
        /// Initial timer before running timer.
        /// </summary>
        public float InitialSecond => _initialTime;

        /// <summary>
        /// Current second shown on timer.
        /// </summary>
        public float OnSecond => _tempSeconds;

        /// <summary>
        /// Is the timer counting up instead of counting down.
        /// </summary>
        public bool IsCountingUp => _isCountingUp;

        #endregion

        #region Main

        /// <summary>
        /// Start running the timer.
        /// </summary>
        public void Start() => _isRunning = true;

        /// <summary>
        /// Ticking timer.
        /// </summary>
        /// <param name="deltaTime">Seconds time per frame</param>
        public void Tick(float deltaTime)
        {
            // Check timer is running, if not then ignore ticking.
            if (!_isRunning) return;

            // Calculate tick time.
            _tempSeconds += (_isCountingUp ? 1f : -1f) * deltaTime * _multiplier;

            // Check times up when count down or count up.
            if ((!_isCountingUp && _tempSeconds <= _timesUpOn) || (_isCountingUp && _tempSeconds >= _timesUpOn))
            {
                // Stop timer.
                _isRunning = false;

                // Call an event.
                OnTimesUp?.Invoke();
            }
        }

        /// <summary>
        /// Reset back to initial time.
        /// </summary>
        /// <param name="stopTimer">Should the timer stopped?</param>
        public void Reset(bool stopTimer = true)
        {
            _tempSeconds = _initialTime;
            _isRunning = !stopTimer;
        }

        /// <summary>
        /// Reset back at second time.
        /// </summary>
        /// <param name="atSecond">Target time reset</param>
        /// <param name="stopTimer">Should the timer stopped?</param>
        public void Reset(float atSecond, bool stopTimer = true)
        {
            _tempSeconds = atSecond;
            _isRunning = !stopTimer;
        }

        #endregion
    }
}
