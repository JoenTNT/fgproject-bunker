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
        private AbstractMovement2DFunc _movementFunc = null;

        [SerializeField]
        private DampingRotation2DFunc _rotationFunc = null;

        // Runtime variable data.
        private bool _isMoving = false;
        private bool _isLookingAround = false;
        private bool _isUsingWeapon = false;

        #endregion

        #region Properties

        /// <summary>
        /// Move function.
        /// </summary>
        public AbstractMovement2DFunc MoveFunc => _movementFunc;

        /// <summary>
        /// Look function.
        /// </summary>
        public DampingRotation2DFunc RotateFunc => _rotationFunc;

        /// <summary>
        /// Shot focus the camera at this point.
        /// </summary>
        public Transform CameraFocusPoint => _cameraFocusPoint;

        /// <summary>
        /// Player control is moving state.
        /// </summary>
        public bool IsMoving
        {
            get => _isMoving;
            set => _isMoving = value;
        }

        /// <summary>
        /// Player control is looking around state.
        /// </summary>
        public bool IsLookingAround
        {
            get => _isLookingAround;
            set => _isLookingAround = value;
        }

        /// <summary>
        /// Player control is using weapon state.
        /// </summary>
        public bool IsUsingWeapon
        {
            get => _isUsingWeapon;
            set => _isUsingWeapon = value;
        }

        #endregion
    }
}
