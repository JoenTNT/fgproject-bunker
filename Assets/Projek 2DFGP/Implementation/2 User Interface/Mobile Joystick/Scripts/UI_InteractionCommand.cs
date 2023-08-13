using JT.GameEvents;
using UnityEngine;
using UnityEngine.UI;

namespace JT.FGP
{
    [RequireComponent(typeof(RectTransform))]
    public class UI_InteractionCommand : MonoBehaviour
    {
        #region Variable

        [SerializeField]
        private UI_InteractionCommandData _data = null;

        [Header("Game Events")]
        [SerializeField]
        private GameEventStringInt _openInteractionCommandCallback = null;

        [SerializeField]
        private GameEventStringInt _onNotifyInteractionHint = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events
            _openInteractionCommandCallback.AddListener(ListenOpenInteractionCommandCallback);
        }

        private void Start()
        {
            // Initially disabled
            _data.UIGroup.alpha = _data.DisabledAlpha;
            _data.SetInteractionIcon(-1);
            _onNotifyInteractionHint.Invoke(_data.TargetID, -1);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _openInteractionCommandCallback.RemoveListener(ListenOpenInteractionCommandCallback);
        }

        #endregion

        #region Main

        private void ListenOpenInteractionCommandCallback(string byEntityID, int commandIndex)
        {
            if (_data.TargetID != byEntityID) return;

            bool found = _data.SetInteractionIcon(commandIndex);
            _data.UIGroup.alpha = found ? 1f : _data.DisabledAlpha;
            _onNotifyInteractionHint.Invoke(byEntityID, commandIndex);
        }

        #endregion
    }

    /// <summary>
    /// Handle data for UI Interaction Command.
    /// </summary>
    [System.Serializable]
    internal class UI_InteractionCommandData : ITargetID<string>
    {
        #region Variable

        [SerializeField]
        private string _targetID = string.Empty;

        [SerializeField]
        private UI_CommandControl _commandControl = null;

        [SerializeField]
        private CanvasGroup _uiGroup = null;

        [SerializeField]
        private Image _interactionIcon = null;

        [SerializeField]
        private Sprite[] _iconSprites = new Sprite[0];

        [SerializeField]
        private float _disableAlpha = 0.35f;

        #endregion

        #region Properties

        /// <summary>
        /// UI Group.
        /// </summary>
        public CanvasGroup UIGroup => _uiGroup;

        /// <summary>
        /// Interaction command control.
        /// </summary>
        public ICommand CommandControl => _commandControl;

        /// <summary>
        /// Alpha transparent when interaction command disabled.
        /// </summary>
        public float DisabledAlpha => _disableAlpha;

        #endregion

        #region Main

        public bool SetInteractionIcon(int index)
        {
            if (index < 0 || index >= _iconSprites.Length)
            {
                _interactionIcon.gameObject.SetActive(false);
                return false;
            }

            _interactionIcon.sprite = _iconSprites[index];
            _interactionIcon.gameObject.SetActive(true);
            return true;
        }

        #endregion

        #region ITargetID

        public string TargetID
        {
            get => _targetID;
            set => _targetID = value;
        }

        #endregion
    }
}
