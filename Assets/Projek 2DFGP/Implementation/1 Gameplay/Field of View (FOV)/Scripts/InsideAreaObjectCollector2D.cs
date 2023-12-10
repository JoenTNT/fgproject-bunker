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

        [Header("Debug")]
        [SerializeField]
        private List<GameObject> _inAreaObjects = new();

        // Temporary variable data
        private GameObject _tempNearestTarget = null;
        private float _tempDistance = 0f;
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
            GameObject obj = collision.gameObject;
            if ((_limitTarget & LimitTarget.Layer) == LimitTarget.Layer)
                _tempCheck &= _targetLayer == (_targetLayer | (1 << obj.layer));

            // Check tag target
            if ((_limitTarget & LimitTarget.Tag) == LimitTarget.Tag)
                _tempCheck &= obj.CompareTag(_targetTag);

            if (!_tempCheck) return;

            _inAreaObjects.Add(obj);
            OnEnterArea?.Invoke(obj);
#if UNITY_EDITOR
            //Debug.Log(_tempCheck
            //    ? $"Successfully Added: {collision}"
            //    : $"Failed to add {collision}, object is not Valid.");
#endif
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            GameObject obj = collision.gameObject;
            bool removed = _inAreaObjects.Remove(obj);

            if (!removed) return;

            OnExitArea?.Invoke(obj);
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
            // Check empty container.
            if (_inAreaObjects.Count == 0) return null;

            _tempNearestTarget = null;
            float nearestDist = float.MaxValue;
            foreach (var tempObject in _inAreaObjects)
            {
                _tempDistance = Vector3.Distance(fromPosition, tempObject.transform.position);
                if (_tempNearestTarget != null && nearestDist <= _tempDistance) continue;

                _tempNearestTarget = tempObject;
                nearestDist = _tempDistance;
            }

            return _tempNearestTarget;
        }

        /// <summary>
        /// Search for the nearest object using custom validator instead.
        /// </summary>
        /// <param name="fromPosition">Position point to target object</param>
        /// <param name="validationFunc">Game Object validator</param>
        /// <returns>Nearest object from vector position</returns>
        public GameObject GetNearestObject(Vector2 fromPosition, System.Func<GameObject, bool> validationFunc)
        {
            // Check empty container.
            if (_inAreaObjects.Count == 0) return null;

            _tempNearestTarget = null;
            float nearestDist = float.MaxValue;
            bool foundAny = false;
            foreach (var tempObject in _inAreaObjects)
            {
                if (!validationFunc(tempObject)) continue;

                foundAny = true;
                _tempDistance = Vector3.Distance(fromPosition, tempObject.transform.position);
                if (_tempNearestTarget != null && nearestDist <= _tempDistance) continue;

                _tempNearestTarget = tempObject;
                nearestDist = _tempDistance;
            }

            // After validation, none of them are valid, so then return null.
            if (!foundAny) return null;

            return _tempNearestTarget;
        }

        /// <summary>
        /// Search for the nearest object using custom validator instead.
        /// </summary>
        /// <param name="fromPosition">Position point to target object</param>
        /// <param name="validationFunc">Game Object validator including "fromPosition"</param>
        /// <returns>Nearest object from vector position</returns>
        public GameObject GetNearestObject(Vector2 fromPosition, System.Func<Vector2, GameObject, bool> validationFunc)
        {
            // Check empty container.
            if (_inAreaObjects.Count == 0) return null;

            _tempNearestTarget = null;
            float nearestDist = float.MaxValue;
            bool foundAny = false;
            foreach (var tempObject in _inAreaObjects)
            {
                if (!validationFunc(fromPosition, tempObject)) continue;

                foundAny = true;
                _tempDistance = Vector3.Distance(fromPosition, tempObject.transform.position);
                if (_tempNearestTarget != null && nearestDist <= _tempDistance) continue;

                _tempNearestTarget = tempObject;
                nearestDist = _tempDistance;
            }

            // After validation, none of them are valid, so then return null.
            if (!foundAny) return null;

            return _tempNearestTarget;
        }

        #endregion
    }
}
