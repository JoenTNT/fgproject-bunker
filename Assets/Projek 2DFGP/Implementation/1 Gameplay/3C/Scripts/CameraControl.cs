using Cinemachine;
using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    [RequireComponent(typeof(Camera))]
    public class CameraControl : MonoBehaviour
    {
        #region Variable

        [SerializeField]
        private CameraControlData _data = null;

        [Header("Game Events")]
        [SerializeField]
        private GameEventTransform _setFollowTargetCallback = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events
            _setFollowTargetCallback.AddListener(ListenSetFollowTargetCallback);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _setFollowTargetCallback.RemoveListener(ListenSetFollowTargetCallback);
        }

        #endregion

        #region Main

        private void ListenSetFollowTargetCallback(Transform target)
        {
            if (target == null) return;

            _data.VirtualCamera.Follow = target;
        }

        #endregion
    }

    /// <summary>
    /// Handle data for camera control.
    /// </summary>
    [System.Serializable]
    internal class CameraControlData
    {
        #region Variable

        [SerializeField]
        private CinemachineVirtualCamera _virtualCameraRef = null;

        #endregion

        #region Properties

        /// <summary>
        /// Virtual camera from cinemachine.
        /// </summary>
        public CinemachineVirtualCamera VirtualCamera => _virtualCameraRef;

        #endregion
    }
}
