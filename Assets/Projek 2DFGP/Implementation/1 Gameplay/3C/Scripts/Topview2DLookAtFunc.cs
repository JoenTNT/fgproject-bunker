using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle Topdown 2D Game for entity rotation.
    /// </summary>
    public class Topview2DLookAtFunc : MonoBehaviour
    {
        #region enum

        /// <summary>
        /// Look at function process mode.
        /// </summary>
        public enum Mode { Fixed = 0, Damping, }

        #endregion

        #region Variable

        [Header("Requirements")]
        [SerializeField]
        private Transform _pivotRotation = null;

        [Header("Properties")]
        [SerializeField]
        private Mode _lookMode = Mode.Fixed;

        //[SerializeField]
        //private float _damping = 1f;

        // Runtime variable data.
        private Vector2 _lookDirection = Vector2.zero;
        private float _lookDegree = 0f;
        //private float _currentRotateSpeed = 0f;

        #endregion

        #region Properties

        /// <summary>
        /// Current topdown look direction information.
        /// </summary>
        public Vector2 LookDirection => _lookDirection;

        #endregion

        #region Main

        /// <summary>
        /// To set look mode process.
        /// Look mode process identical to force mode of rigidbody.
        /// </summary>
        /// <param name="mode">Select look mode</param>
        public void SetLookMode(Mode mode) => _lookMode = mode;

        /// <summary>
        /// Look at direction using position target.
        /// </summary>
        /// <param name="lookPosition">Look at position target</param>
        public void LookAtPosition(Vector2 lookPosition)
        {
            Vector2 toVector2 = new Vector2(_pivotRotation.position.x, _pivotRotation.position.y);
            LookAtDirection((lookPosition - toVector2).normalized);
        }

        /// <summary>
        /// Look at target direction.
        /// </summary>
        /// <param name="lookDirection">Target direction</param>
        public void LookAtDirection(Vector2 lookDirection)
        {
            _lookDirection = lookDirection;
            _lookDegree = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

            // Check which mode is being used.
            switch (_lookMode)
            {
                case Mode.Damping:
                    float angleDiff = Mathf.DeltaAngle(_pivotRotation.eulerAngles.z, _lookDegree);
                    // TODO: Smooth damping rotation process.
                    break;

                default: // Fixed
                    _pivotRotation.rotation = Quaternion.Euler(0f, 0f, _lookDegree);
                    break;
            }
        }

        #endregion
    }
}
