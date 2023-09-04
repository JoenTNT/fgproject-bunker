using JT.GameEvents;
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
        private EntityID _elementID = null;

        [SerializeField]
        private CanvasGroup _canvasGroup = null;

        [SerializeField]
        private Image _placeholder = null;

        [Header("Properties")]
        [SerializeField]
        private float _onEmptyAlpha = 0.45f;

        [SerializeField]
        private bool _disableOnStart = false;

        [Header("Game Events")]
        [SerializeField]
        private GameEventStringUnityObject _changeSpriteImageCallback = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Disable when start.
            _placeholder.enabled = !_disableOnStart;

            // Subscribe events.
            _changeSpriteImageCallback.AddListener(ListenChangeSpriteImageCallback);
        }

        private void OnDestroy()
        {
            // Unsubscribe events.
            _changeSpriteImageCallback.RemoveListener(ListenChangeSpriteImageCallback);
        }

        #endregion

        #region Main

        private void ListenChangeSpriteImageCallback(string elementID, Object sprite)
        {
            // Check id and make sure object is a sprite, if not then abort process.
            if (_elementID.ID != elementID) return;
            if (sprite != null && sprite is not Sprite) return;

            // Set sprite content to target placeholder.
            _placeholder.sprite = sprite == null ? null : (Sprite)sprite;

            // Enable or disable object when null or not.
            _placeholder.enabled = sprite != null;
            _canvasGroup.alpha = _placeholder.enabled ? 1f : _onEmptyAlpha;
        }

        #endregion
    }
}
