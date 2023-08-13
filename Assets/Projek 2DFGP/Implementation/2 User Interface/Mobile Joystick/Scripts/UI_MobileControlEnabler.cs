using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// This script is specifically only to enable Mobile Controller in Mobile Platform.
    /// This will Destroy in the other Platforms.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UI_MobileControlEnabler : MonoBehaviour
    {
        #region Variable

        [Header("Game Events")]
        [SerializeField]
        private GameEventBool _lockControllerCallback = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events
            _lockControllerCallback.AddListener(ListenLockControllerCallback);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _lockControllerCallback.RemoveListener(ListenLockControllerCallback);
        }

        private void Start()
        {
            // TODO: Enable in Mobile and Destroy in Other Platform.
        }

        #endregion

        #region Main

        private void ListenLockControllerCallback(bool isLock) => gameObject.SetActive(!isLock);

        #endregion
    }
}
