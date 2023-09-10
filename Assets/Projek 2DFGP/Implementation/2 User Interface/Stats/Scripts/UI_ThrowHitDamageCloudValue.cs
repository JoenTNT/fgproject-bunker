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
        public RectTransform rectTransform
        {
            get
            {
                if (_rectTransform != null) return _rectTransform;
                return _rectTransform = (RectTransform)transform;
            }
        }

        #endregion

        #region Mono

        private void Start()
        {
            // Disable object on start if needed.
            if (_disableOnStart)
                gameObject.SetActive(false);

            //// TEMP: Run on start for testing.
            //RectTransform parent = (RectTransform)rectTransform.parent.transform;
            //Run(parent.rect.center);
        }

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
                gameObject.SetActive(true);

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

