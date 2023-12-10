using System.Collections;
using TMPro;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Throw value of hit damage from entity.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UI_ThrowHitDamageCloudValue : MonoBehaviour
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private TextMeshProUGUI _valueText = null;

        [Header("Runtime Properties")]
        [SerializeField]
        private Vector2 _placingPosition = Vector2.zero;

        [Header("Properties")]
        [Tooltip("Animation curve moveing to y axis.")]
        [SerializeField]
        private AnimationCurve _yCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [SerializeField]
        private Vector2 _placementOffset = Vector2.zero;

        [SerializeField]
        private float _yFarJump = 100f;

        [SerializeField]
        private bool _disableOnStart = true;

        // Runtime variable data.
        private RectTransform _rectTransform = null;
        private IEnumerator _routine = null;

        #endregion

        #region Properties

        /// <summary>
        /// Rectangle transform of this game object.
        /// </summary>
        public RectTransform rectTransform => _rectTransform;

        #endregion

        #region Mono

        private void Awake()
        {
            // Initialize data.
            _rectTransform = (RectTransform)transform;
        }

        private void Start()
        {
            // Disable object on start if needed.
            if (_disableOnStart)
            {
                // Delete routine on start when accidentally ran.
                if (_routine != null)
                {
                    StopCoroutine(_routine);
                    _routine = null;
                }

                // Disable object.
                gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            // Always run on enable.
            Run(_placingPosition, false);
        }

        private void OnDisable()
        {
            // Always delete routine on disable when routine not yet finished.
            if (_routine != null)
            {
                StopCoroutine(_routine);
                _routine = null;
            }
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Debug.DrawRay(_placingPosition + _placementOffset, new Vector3(0f, _yFarJump, 0f), Color.green);
            Gizmos.DrawWireSphere(_placingPosition + _placementOffset, 0.25f);
        }
#endif
        #endregion

        #region Main

        public void SetValue(float hitDamage)
        {
            // TODO: Decimal value precision settings.
            _valueText.text = $"{(int)hitDamage}";
        }

        public void Run(Vector2 startAnchorPoint, bool activateObject = true)
        {
            // Check if game object is not active yet, then set active it.
            if (activateObject && !gameObject.activeSelf)
            {
                // Enable the game object before running routine.
                _placingPosition = startAnchorPoint;
                gameObject.SetActive(true);
                return;
            }

            // Stop existing routine, always interupt it.
            if (_routine != null)
            {
                StopCoroutine(_routine);
                _routine = null;
            }

            // Create a new routine.
            _routine = RunRoutine(startAnchorPoint + _placementOffset, _yCurve);
            StartCoroutine(_routine);
        }

        private IEnumerator RunRoutine(Vector2 startAnchorPoint, AnimationCurve yCurve)
        {
            // Initialize starting values.
            Vector3 currentAnchorPos = rectTransform.anchoredPosition = startAnchorPoint;
            float currentTimer = 0f, yPercentValue;
            float endTimer = yCurve.keys[yCurve.length - 1].time; // End time always from animation curve.

            // Start process.
            while (currentTimer < endTimer)
            {
                // Tick time.
                currentTimer += Time.deltaTime;

                // Wait for next frame.
                yield return null;

                // Set end timer precision.
                if (currentTimer >= endTimer)
                    currentTimer = endTimer;

                // Process position.
                yPercentValue = yCurve.Evaluate(currentTimer);
                currentAnchorPos.y = startAnchorPoint.y + (yPercentValue * _yFarJump);
                rectTransform.anchoredPosition = currentAnchorPos;
            }

            // End of process.
            _routine = null;
            gameObject.SetActive(false);
        }

        #endregion
    }
}

