using System.Collections.Generic;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// A function component to collect all object inside the area.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class InsideAreaObjectCollector2D : MonoBehaviour
    {
        #region event

        /// <summary>
        /// Event called when an object entered the area.
        /// </summary>
        public event System.Action<GameObject> OnEnterArea;

        /// <summary>
        /// Event called when an object exited the area.
        /// </summary>
        public event System.Action<GameObject> OnExitArea;

        #endregion

        #region enum

        /// <summary>
        /// Limit track the target object inside the area.
        /// </summary>
        [System.Flags]
        private enum LimitTarget
        {
            None = 0,
            Tag = 1,
            Layer = 2,
        }

        #endregion

        #region Variable

        [SerializeField]
        private LimitTarget _limitTarget = LimitTarget.None;

        [SerializeField]
        private LayerMask _targetLayer = ~0;

        [SerializeField]
        private string _targetTag = string.Empty;

        [SerializeField]
        [ReadOnly, Space]
        private HashSet<GameObject> _inAreaObjects = new();

        // Temporary variable data
        private GameObject _tempNearestTarget = null;
        private bool _tempCheck = false;

        #endregion

        #region Properties

        /// <summary>
        /// Is there any object detected inside the area.
        /// </summary>
        public bool HasObject => _inAreaObjects.Count > 0;

        #endregion

        #region Mono

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _tempCheck = true;

            // Check layer target
            if ((_limitTarget & LimitTarget.Layer) == LimitTarget.Layer)
                _tempCheck = _targetLayer == (_targetLayer | (1 << collision.gameObject.layer));

            // Check tag target
            if ((_limitTarget & LimitTarget.Tag) == LimitTarget.Tag)
                _tempCheck = collision.gameObject.CompareTag(_targetTag);

            if (!_tempCheck) return;

            _inAreaObjects.Add(collision.gameObject);
            OnEnterArea?.Invoke(collision.gameObject);
#if UNITY_EDITOR
            //Debug.Log(_tempCheck
            //    ? $"Successfully Added: {collision}"
            //    : $"Failed to add {collision}, object is not Valid.");
#endif
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            bool removed = _inAreaObjects.Remove(collision.gameObject);

            if (!removed) return;

            OnExitArea?.Invoke(collision.gameObject);
#if UNITY_EDITOR
            //Debug.Log(removed
            //    ? $"Successfully Removed: {collision}"
            //    : $"Failed to remove {collision}, object may not be found.");
#endif
        }

        #endregion

        #region Main

        /// <summary>
        /// Search for the nearest object.
        /// </summary>
        /// <param name="fromPosition">Position point to target object</param>
        /// <returns>Nearest object from vector position</returns>
        public GameObject GetNearestObject(Vector2 fromPosition)
        {
            float nearestDistance = float.MaxValue;
            foreach (var tempObject in _inAreaObjects)
            {
                float distance = Vector3.Distance(fromPosition, tempObject.transform.position);
                if (_tempNearestTarget != null && nearestDistance <= distance) continue;

                _tempNearestTarget = tempObject;
                nearestDistance = distance;
            }

            if (_inAreaObjects.Count == 0)
                _tempNearestTarget = null;

            return _tempNearestTarget;
        }

        /// <summary>
        /// Search for the nearest object.
        /// </summary>
        /// <param name="fromPosition">Position point to target object</param>
        /// <param name="validationFunc">Nearest object validator</param>
        /// <returns>Nearest object from vector position</returns>
        public GameObject GetNearestObject(Vector2 fromPosition, System.Func<GameObject, bool> validationFunc)
        {
            return null;
        }

        #endregion
    }
}
