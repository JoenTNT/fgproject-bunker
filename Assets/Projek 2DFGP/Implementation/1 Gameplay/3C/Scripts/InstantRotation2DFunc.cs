using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle instant rotation setter for 2D game.
    /// </summary>
    public class InstantRotation2DFunc : MonoBehaviour
    {
        #region Variables

        [Header("Base Requirements")]
        [SerializeField]
        private Transform _pivotRotation = null;

        [Header("Base Properties")]
        [SerializeField]
        private Vector3 _eulerOffset = Vector3.zero;

        [SerializeField]
        [Tooltip("Only works if the target pivot rotation object is child of any parent.")]
        private bool _normalizeByParent = false;

        #endregion

        #region Properties

        /// <summary>
        /// Which transform must be rotate.
        /// </summary>
        protected Transform PivotTransform => _pivotRotation;

        #endregion

        #region Main

        /// <summary>
        /// Set look at position instantly.
        /// </summary>
        /// <param name="lookAtPos">Target position to look at</param>
        public void SetInstantLookAtPosition(Vector2 lookAtPos)
            => SetInstantLookDirection(lookAtPos - (Vector2)_pivotRotation.position);

        /// <summary>
        /// Set look direction instantly.
        /// </summary>
        /// <param name="lookDir">Target look direction</param>
        public void SetInstantLookDirection(Vector2 lookDir)
            => SetInstantRotationRadian(Mathf.Atan2(lookDir.y, lookDir.x));

        /// <summary>
        /// Set rotation using Z degree instantly.
        /// </summary>
        /// <param name="zDegree">Target rotation degree</param>
        public void SetInstantRotationDegree(float zDegree)
        {
            // Set rotation by euler angle degree, then rotate by offset.
            _pivotRotation.rotation = Quaternion.Euler(0f, 0f, zDegree);
            _pivotRotation.Rotate(_eulerOffset);

            // Normalize by parent rotation if needed.
            if (_normalizeByParent && _pivotRotation.parent != null)
                _pivotRotation.Rotate(-_pivotRotation.parent.eulerAngles);
        }

        /// <summary>
        /// Set rotation using Z radian instantly.
        /// </summary>
        /// <param name="zRadian">Target rotation radian</param>
        public void SetInstantRotationRadian(float radian)
            => SetInstantRotationDegree(radian * Mathf.Rad2Deg);

        #endregion

        #region Statics

        /// <summary>
        /// Used to sync degree between -360 to 360 degree.
        /// </summary>
        protected static void SyncShortestPath(ref float currentZDegree, ref float targetZDegree)
        {
            // Check degree range less than 180 degree, then ignore because it's the shortest.
            if (Mathf.Abs(targetZDegree - currentZDegree) <= 180f) return;

            //Debug.Log($"Target: {targetZDegree}; Current: {currentZDegree}");
        }

        #endregion
    }
}
