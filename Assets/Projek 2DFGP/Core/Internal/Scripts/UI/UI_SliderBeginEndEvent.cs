using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JT
{
    [RequireComponent(typeof(Slider))]
    public class UI_SliderBeginEndEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        #region Variable

        [Header("Events")]
        [SerializeField]
        private UnityEvent<float> _onBeginValueChanged = null;

        [SerializeField]
        private UnityEvent<float> _onEndValueChanged = null;

        // Temporary variable data
        private Slider _slider = null;

        #endregion

        #region Mono

        private void Awake()
        {
            if (_slider == null)
                TryGetComponent(out _slider);
        }

        #endregion

        #region IPointerDownHandler

        public void OnPointerDown(PointerEventData eventData)
        {
            _onBeginValueChanged?.Invoke(_slider.value);
        }

        #endregion

        #region IPointerUpHandler

        public void OnPointerUp(PointerEventData eventData)
        {
            _onEndValueChanged?.Invoke(_slider.value);
        }

        #endregion
    }
}
