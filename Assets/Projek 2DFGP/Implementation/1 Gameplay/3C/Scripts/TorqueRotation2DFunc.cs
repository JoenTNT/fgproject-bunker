using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle torque rotation 2D.
    /// </summary>
    public class TorqueRotation2DFunc : InstantRotation2DFunc, IHandleRotation2D, ITargetRotation2D
    {
        #region Variables

        [Header("Properties")]
        [Tooltip("Rotation speed in Degree.")]
        [SerializeField]
        private float _rotationSpeed = 30f;

        [SerializeField]
        private bool _shortestPath = true;

        // Runtime variable data.
        private float _currentDegree = 0f;
        private float _targetDegree = 0f;

        #endregion

        #region Properties

        /// <summary>
        /// Rotation speed in degree.
        /// </summary>
        public float RotationSpeed
        {
            get => _rotationSpeed;
            set => _rotationSpeed = value;
        }

        #endregion

        #region Mono

        private void Awake()
        {
            // Set runtime initial values.
            _targetDegree = _currentDegree = PivotTransform.eulerAngles.z;
        }

        #endregion

        #region IHandleRotation2D

        public void OnRotate()
        {

        }

        #endregion

        #region ITargetRotation2D

        public void SetTargetLookAtPosition(Vector2 lookAtPos)
            => SetTargetLookDirection(lookAtPos - (Vector2)transform.position);

        public void SetTargetLookDirection(Vector2 lookDir)
            => SetTargetRotationRadian(Mathf.Atan2(lookDir.y, lookDir.x));

        public void SetTargetRotationDegree(float zDegree)
            => _targetDegree = zDegree;

        public void SetTargetRotationRadian(float zRadian)
            => SetTargetRotationDegree(zRadian * Mathf.Rad2Deg);

        #endregion
    }
}
