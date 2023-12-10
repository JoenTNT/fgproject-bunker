using UnityEngine;
using UnityEngine.UI;

namespace JT.FGP
{
    /// <summary>
    /// Handle updates in UI player profile.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UI_RuntimePlayerStats : MonoBehaviour, IImagePlaceholder<Sprite>
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private Image _pfpPlaceholder = null;

        [SerializeField]
        private UI_HitpointBar _healthBar = null;

        // Runtime variable data.
        private RectTransform _rectTransform = null;

        #endregion

        #region Properties

        public RectTransform rectTransform => _rectTransform;

        /// <summary>
        /// Health bar value.
        /// </summary>
        public IBarValue<float> HealthBar => _healthBar;

        #endregion

        #region Mono

        private void Awake()
        {
            // Initialize data.
            _rectTransform = (RectTransform)transform;
        }

        #endregion

        #region IImagePlaceholder

        /// <summary>
        /// Set PFP image.
        /// </summary>
        public void SetImage(Sprite image) => _pfpPlaceholder.sprite = image;

        #endregion
    }
}