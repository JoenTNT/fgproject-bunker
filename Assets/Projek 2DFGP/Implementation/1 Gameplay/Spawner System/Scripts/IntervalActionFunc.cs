using UnityEngine;
using UnityEngine.Events;

namespace JT.FGP
{
    /// <summary>
    /// Call action every interval time loop.
    /// </summary>
    public sealed class IntervalActionFunc : MonoBehaviour
    {
        #region Variables

        [Header("Properties")]
        [SerializeField]
        private Timer _timer = new Timer();

        [SerializeField]
        private bool _doActionOnStart = false;

        [Header("Game Events")]
        [SerializeField]
        private UnityEvent _doActions = new UnityEvent();

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events.
            _timer.OnTimesUp += ListenOnTimesUp;
        }

        private void OnDestroy()
        {
            // Unsubscribe events.
            _timer.OnTimesUp -= ListenOnTimesUp;
        }

        private void Start()
        {
            // Check do action on start.
            if (_doActionOnStart)
                _doActions?.Invoke();

            // Start timer.
            _timer.Reset();
            _timer.Start();
        }

        private void Update()
        {
            // Tick timer.
            _timer.Tick(Time.deltaTime);
        }

        #endregion

        #region Main

        private void ListenOnTimesUp()
        {
            // Call action.
            _doActions?.Invoke();

            // Restart timer.
            _timer.Reset();
            _timer.Start();
        }

        #endregion
    }
}
