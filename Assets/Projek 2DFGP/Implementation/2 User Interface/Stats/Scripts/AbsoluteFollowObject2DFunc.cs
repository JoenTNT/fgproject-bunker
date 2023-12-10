using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle following an object target position.
    /// </summary>
    public class AbsoluteFollowObject2DFunc : MonoBehaviour
        //, IDelayAction<float>
    {
        #region Variables

        [Header("Properties")]
        [SerializeField]
        private Vector2 _offsetPosition = Vector2.zero;

        [SerializeField]
        private float _offsetZRotation = 0f;

        [SerializeField]
        private bool _followPosition = true;

        [SerializeField]
        private bool _followRotation = false;

        //[SerializeField, Min(0f)]
        //private float _delaySecond = 0f;

        //[Tooltip("Distance between this object with the follow object that flagged as arrived.")]
        //[SerializeField, Min(0f)]
        //private float _arrivalDistance = 0.01f;

        [Header("Optional")]
        [SerializeField]
        private Transform _followObject = null;

        // Runtime variable data.
        //private float _secondBeforeAction = 0f;
        //private bool _isArrived = true;

        #endregion

        #region Mono

        private void Start()
        {
            // Reset transform of follower.
            transform.rotation = Quaternion.identity;

            // Detach from parent and normalize rotation.
            if (transform.parent != null) transform.SetParent(null);
        }

        private void LateUpdate()
        {
            // Ignore if there's nothing to follow.
            if (_followObject == null) return;

            // Set position immediately.
            if (_followPosition)
                transform.position = (Vector2)_followObject.position + _offsetPosition;

            // Set rotation immediately.
            if (_followRotation)
            {
                float zDegree = _followObject.eulerAngles.z;
                zDegree += _offsetZRotation;
                _followObject.eulerAngles = new Vector3(0f, 0f, zDegree);
            }

            //// If object has arrived, then ignore the process.
            //Vector2 followPos = _followObject.position;
            //Vector2 currentPos = transform.position;
            //if (_isArrived || _secondBeforeAction > 0f)
            //{
            //    // Check distance to retract arrival status.
            //    _isArrived = CalculateDistance(currentPos, followPos) <= _arrivalDistance;
            //    if (!_isArrived && _secondBeforeAction <= 0f)
            //        _secondBeforeAction = _delaySecond;
            //    if (_secondBeforeAction > 0f)
            //        _secondBeforeAction -= Time.deltaTime;
            //    return;
            //}
        }

        #endregion

        //#region IDelayAction

        //public float InitialDelaySecond
        //{
        //    get => _delaySecond;
        //    set => _delaySecond = value;
        //}

        //public float SecondBeforeAction => _secondBeforeAction;

        //public void ResetDelay()
        //{
        //    // Ignore process if not running.
        //    if (_secondBeforeAction <= 0f) return;

        //    // Reset delay.
        //    _secondBeforeAction = _delaySecond;
        //}

        //#endregion

        #region Main

        /// <summary>
        /// Set follow target object.
        /// </summary>
        /// <param name="follow">Target object transform</param>
        public void SetFollow(Transform follow)
        {
            _followObject = follow;
            //_isArrived = false;
        }

        #endregion

        //#region Statics

        //private float CalculateDistance(Vector2 from, Vector2 to) => Vector2.Distance(from, to);

        //#endregion
    }
}
