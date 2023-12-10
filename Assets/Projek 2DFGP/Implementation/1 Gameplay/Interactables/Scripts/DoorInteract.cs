using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace JT.FGP
{
    /// <summary>
    /// Handle open and close interaction with door.
    /// </summary>
    public class DoorInteract : InteractableComponent, IClosable
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private Transform _pivotPoint = null;

        [Header("Properties")]
        [SerializeField]
        private float _tweenDuration = .35f;

        [SerializeField]
        private float _openDegree = 100f;

        [SerializeField]
        private bool _isClosable = true;

        //[Tooltip("Inside Rect area to detect the entity who open the door. " +
        //    "Direction will always negative degree value.")]
        //[SerializeField]
        //private Rect _insideRect = new Rect(1f, 1f, 1.8f, 1.2f);

        //[Tooltip("Outside Rect area to detect the entity who open the door. " +
        //    "Direction will always positive degree value.")]
        //[SerializeField]
        //private Rect _outsideRect = new Rect(1f, -1f, 1.8f, 1.2f);

        //[SerializeField]
        //private LayerMask _entityLayer = ~0;

        //[SerializeField, Min(1)]
        //private int _maxEntityCheck = 5;

        // Runtime variable data.
        private float _originDegree = 0f;
        private bool _isOpened = false;
        //private IEnumerator _animateRoutine = null;
        //private RaycastHit2D[] _insideHits = null;
        //private RaycastHit2D[] _outsideHits = null;
        //private float _originDegree = 0f;
        //private bool _openOut = false;

        #endregion

        #region Mono

        private void Awake()
        {
            //// Init max entity hits.
            //_insideHits = new RaycastHit2D[_maxEntityCheck];
            //_outsideHits = new RaycastHit2D[_maxEntityCheck];

            // Check starter interactable status.
            if (_isOpened && !_isClosable) IsInteractable = false;

            // Set initial degree pivot.
            _originDegree = _pivotPoint.eulerAngles.z;
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_isClosable && !IsInteractable)
                IsInteractable = true;
        }

        private void OnDrawGizmosSelected()
        {
            // Debug maximum hinge.
            Vector2 defaultDir = Vector2.right, currentPos = transform.right;
            Debug.DrawRay(currentPos, RotateDirByDegree(defaultDir, _openDegree), Color.cyan);
        }
#endif
        #endregion

        #region InteractableComponent

        public override InteractionType Type => _isOpened ? InteractionType.CloseDoor
            : InteractionType.OpenDoor;

        public override bool Interact(string entity)
        {
            // Check closable.
            if (_isOpened && !_isClosable) return false;

            // Declare requirements.
            float targetDegree;

            // Check the door is open or closed.
            if (_isOpened) targetDegree = _originDegree;
            else targetDegree = _originDegree + _openDegree;

            // Run tween.
            _pivotPoint.DORotate(new Vector3(0f, 0f, targetDegree), _tweenDuration);
            if (_isClosable) _isOpened = !_isOpened;
            else
            {
                _isOpened = true;
                IsInteractable = false;
            }
            return true;
        }

        #endregion

        #region IClosable

        public bool IsClosable => _isClosable;

        public void Close()
        {
            // Ignore if already closed.
            if (!_isOpened || !_isClosable) return;

            // Close the door.
            _isOpened = false;
            _pivotPoint.DORotate(new Vector3(0f, 0f, _originDegree), _tweenDuration);
        }

        #endregion

        #region Main

        //private IEnumerator AnimateRoutine()
        //{
        //    yield return null;

        //    //// Declare area info.
        //    //Vector2 currentPos = transform.position;
        //    //Vector2 ip = _insideRect.position + currentPos;
        //    //Vector2 op = _outsideRect.position + currentPos;
        //    //Vector2 insideSize = _insideRect.size;
        //    //Vector2 outsideSize = _outsideRect.size;
        //    //Vector2 his = _insideRect.size / 2f;
        //    //Vector2 hos = _outsideRect.size / 2f;

        //    //// Debug inside area.
        //    //int ic = Physics2D.BoxCastNonAlloc(ip, insideSize, 0f, Vector2.zero,
        //    //    _insideHits, 0f, _entityLayer);
        //    //bool isEntityInteractInside = false;
        //    //for (int i = 0; i < ic; i++)
        //    //{
        //    //    isEntityInteractInside = _insideHits[i].collider.gameObject == EntityObjectTarget;
        //    //    if (isEntityInteractInside) break;
        //    //}
        //    //Color insideColor = isEntityInteractInside ? Color.green : Color.red;
        //    //Debug.DrawRay(ip - his, new Vector2(his.x * 2f, 0f), insideColor, 3f);
        //    //Debug.DrawRay(ip - his, new Vector2(0f, his.y * 2f), insideColor, 3f);
        //    //Debug.DrawRay(ip + his, new Vector2(-his.x * 2f, 0f), insideColor, 3f);
        //    //Debug.DrawRay(ip + his, new Vector2(0f, -his.y * 2f), insideColor, 3f);

        //    //// Debug outside area.
        //    //if (!isEntityInteractInside)
        //    //{
        //    //    int oc = Physics2D.BoxCastNonAlloc(op, outsideSize, 0f, Vector2.zero,
        //    //    _outsideHits, 0f, _entityLayer);
        //    //    Debug.Log($"[DEBUG] There are {ic} object(s) detected.");
        //    //    for (int i = 0; i < ic; i++)
        //    //        Debug.Log(_insideHits[i].collider, _insideHits[i].collider);
        //    //    Color oursideColor = Color.red;
        //    //    Debug.DrawRay(op - hos, new Vector2(hos.x * 2f, 0f), oursideColor, 3f);
        //    //    Debug.DrawRay(op - hos, new Vector2(0f, hos.y * 2f), oursideColor, 3f);
        //    //    Debug.DrawRay(op + hos, new Vector2(-hos.x * 2f, 0f), oursideColor, 3f);
        //    //    Debug.DrawRay(op + hos, new Vector2(0f, -hos.y * 2f), oursideColor, 3f);
        //    //}
        //}

        #endregion

        #region Statics

        private static Vector2 RotateDirByDegree(Vector2 dir, float degree)
        {
            float radian = degree * Mathf.Deg2Rad;
            return new Vector2(dir.x * Mathf.Cos(radian) - dir.y * Mathf.Sin(radian),
                dir.x * Mathf.Sin(radian) + dir.y * Mathf.Cos(radian));
        }

        #endregion
    }
}
