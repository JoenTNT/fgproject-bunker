using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle damping rotation in 2D game.
    /// </summary>
    public class DampingRotation2DFunc : InstantRotation2DFunc, IHandleRotation2D, ITargetRotation2D
    {
        #region Variables

        [Header("Properties")]
        //[SerializeField, Min(0f)]
        //private float _minTorque = 1f;

        [Tooltip("Rotation damping value")]
        [SerializeField, Min(0.01f)]
        private float _damping = 1f;

        [SerializeField, Min(0f)]
        private float _snapDegreeBelow = .01f;

        //[SerializeField]
        //private bool _shortestPath = true;

        // Runtime variable data.
        private float _currentDegree = 0f;
        private float _targetDegree = 0f;

        #endregion

        #region Properties

        ///// <summary>
        ///// After calculating damping to torque,
        ///// the torque must be at minimal if less than expected.
        ///// </summary>
        //public float MinTorque
        //{
        //    get => _minTorque;
        //    set => _minTorque = value;
        //}

        /// <summary>
        /// Damping rotation value.
        /// </summary>
        public float Damping => _damping;

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
            _currentDegree = PivotTransform.eulerAngles.z;
            if (_currentDegree == _targetDegree) return;

            float step = _damping * Time.deltaTime;
            _currentDegree = Mathf.LerpAngle(_currentDegree, _targetDegree, step);

            if (Mathf.Abs(_targetDegree - _currentDegree) < _snapDegreeBelow)
                _currentDegree = _targetDegree;

            SetInstantRotationDegree(_currentDegree);
        }

        #endregion

        #region ITargetRotation2D

        public void SetTargetLookAtPosition(Vector2 lookAtPos)
            => SetTargetLookDirection(lookAtPos - (Vector2)transform.position);

        public void SetTargetLookDirection(Vector2 lookDir)
            => SetTargetRotationRadian(Mathf.Atan2(lookDir.y, lookDir.x));

        public void SetTargetRotationDegree(float zDegree)
        {
            // Assign target Z degree rotation.
            _targetDegree = zDegree;
        }

        public void SetTargetRotationRadian(float zRadian)
            => SetTargetRotationDegree(zRadian * Mathf.Rad2Deg);

        #endregion
    }
}

