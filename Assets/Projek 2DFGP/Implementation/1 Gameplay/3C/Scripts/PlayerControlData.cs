using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle data for player control.
    /// </summary>
    [System.Serializable]
    internal class PlayerControlData
    {
        #region Variable

        [Header("Requirements")]
        [SerializeField]
        private Transform _cameraFocusPoint = null;

        [SerializeField]
        private Topview2DMovementFunc _moveFunc = null;

        [SerializeField]
        private Topview2DLookAtFunc _lookFunc = null;

        #endregion

        #region Properties

        /// <summary>
        /// Move function.
        /// </summary>
        public IMovement2D MoveFunc => _moveFunc;

        /// <summary>
        /// Look function.
        /// </summary>
        public Topview2DLookAtFunc LookFunc => _lookFunc;

        /// <summary>
        /// Shot focus the camera at this point.
        /// </summary>
        public Transform CameraFocusPoint => _cameraFocusPoint;

        #endregion
    }
}
