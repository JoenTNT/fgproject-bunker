using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Using timer to destroy an object.
    /// </summary>
    public sealed class DestroyTimer : MonoBehaviour
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private Timer _timer = new Timer();

        [Header("Properties")]
        [SerializeField]
        private bool _disableOnly = false;

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events
            _timer.OnTimesUp += ListenOnTimesUp;
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _timer.OnTimesUp -= ListenOnTimesUp;
        }

        private void OnEnable()
        {
            _timer.Reset();
            _timer.Start();
        }

        private void Update() => _timer.Tick(Time.deltaTime);

        #endregion

        #region Main

        private void ListenOnTimesUp()
        {
            if (_disableOnly) gameObject.SetActive(false);
            else Destroy(gameObject);
        }

        #endregion
    }
}

