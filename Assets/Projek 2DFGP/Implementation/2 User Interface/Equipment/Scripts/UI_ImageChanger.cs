using UnityEngine;
using UnityEngine.UI;

namespace JT.FGP
{
    /// <summary>
    /// Only receive event to handle image changer.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UI_ImageChanger : MonoBehaviour
    {
        #region Variable

        [Header("Requirements")]
        [SerializeField]
        private CanvasGroup _canvasGroup = null;

        [SerializeField]
        private Image _placeholder = null;

        [Header("Properties")]
        [SerializeField]
        private float _onEmptyAlpha = 0.45f;

        [SerializeField]
        private bool _disableOnStart = false;

        #endregion

        #region Main

        /// <summary>
        /// Change image on UI Image placeholder.
        /// </summary>
        /// <param name="sprite">Using sprite</param>
        public void ChangeImage(Sprite sprite)
        {
            // Set sprite content to target placeholder.
            _placeholder.sprite = sprite == null ? null : sprite;

            // Enable or disable object when null or not.
            _placeholder.enabled = sprite != null;
            _canvasGroup.alpha = _placeholder.enabled ? 1f : _onEmptyAlpha;
        }

        #endregion
    }
}
