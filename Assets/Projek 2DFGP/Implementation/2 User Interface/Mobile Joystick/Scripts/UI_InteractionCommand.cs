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
        private GameEventTwoStringInt _onNotifyInteractionHint = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events
            _onNotifyInteractionHint.AddListener(ListenOnNotifyInteractionHint, this);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _onNotifyInteractionHint.RemoveListener(ListenOnNotifyInteractionHint, this);
        }

        private void Start()
        {
            // Initially disabled
            _data.UIGroup.alpha = _data.DisabledAlpha;
            _data.SetInteractionIcon(-1);
        }

        #endregion

        #region Main

        private void ListenOnNotifyInteractionHint(string whoInteract, string interactWith,
            int commandIndex)
        {
            if (_data.TargetID != whoInteract) return;

            bool found = _data.SetInteractionIcon(commandIndex);
            _data.UIGroup.alpha = found ? 1f : _data.DisabledAlpha;
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
        private InteractionIconsSO _icons = null;

        [SerializeField]
        private UI_CommandControl _commandControl = null;

        [SerializeField]
        private CanvasGroup _uiGroup = null;

        [SerializeField]
        private Image _interactionIcon = null;

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

        #region ITargetID

        public string TargetID
        {
            get => _targetID;
            set => _targetID = value;
        }

        #endregion

        #region Main

        public bool SetInteractionIcon(int index)
        {
            if (index < 0 || index >= _icons.Count)
            {
                _interactionIcon.gameObject.SetActive(false);
                return false;
            }

            _interactionIcon.sprite = _icons.GetIcon((InteractionType)index);
            _interactionIcon.gameObject.SetActive(true);
            return true;
        }

        #endregion
    }
}
