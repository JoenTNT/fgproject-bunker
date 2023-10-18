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
            // Enabler only works in mobile platform.
#if UNITY_ANDROID || UNITY_IOS
            // TODO: Always enable in Mobile platform.
#elif UNITY_STANDALONE && !UNITY_EDITOR // This will Destroy the mobile controller
            // TODO: Always Destroy in Other Platform, except Editor.
#endif
        }

        #endregion

        #region Main

        private void ListenLockControllerCallback(bool isLock) => gameObject.SetActive(!isLock);

        #endregion
    }
}
