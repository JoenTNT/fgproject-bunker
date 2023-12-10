using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Button command control.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UI_CommandControl : MonoBehaviour, ICommand
    {
        #region Variable

        [Header("Requirements")]
        [SerializeField]
        private EntityID _commandID = null;

        [SerializeField]
        private MonoBehaviour _button = null;

        [Header("Properties")]
        [SerializeField]
        private string _targetID = string.Empty;

        [Header("Optional")]
        [SerializeField]
        private CanvasGroup _alphaGroup = null;

        [SerializeField, Min(0f)]
        private float _onDisableAlpha = 0f;

        [Header("Game Events")]
        [SerializeField]
        private GameEventString _onCommand = null;

        [SerializeField]
        private GameEventStringBool _setActiveCommand = null;

        // Runtime variable data.
        private float _originAlpha = 0f;
        private bool _isDisabled = false;

        #endregion

        #region Mono

        private void Awake()
        {
            // Set initial data.
            if (_alphaGroup != null)
                _originAlpha = _alphaGroup.alpha;

            // Subscribe events
            ((IClickEvent)_button).OnClick.AddListener(RunCommand);
            _setActiveCommand.AddListener(ListenSetActiveCommand);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            ((IClickEvent)_button).OnClick.RemoveListener(RunCommand);
            _setActiveCommand.RemoveListener(ListenSetActiveCommand);
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_button is not IClickEvent)
            {
                _button = null;
                Debug.LogWarning("[DEBUG] Object must implement \"IClickEvent\".", this);
            }
        }
#endif
        #endregion

        #region ICommand

        public void RunCommand()
        {
            // Check command callback is disabled.
            if (_isDisabled) return;

            // Run command by calling event.
            _onCommand.Invoke(_targetID);
        }

        #endregion

        #region Main

        private void ListenSetActiveCommand(string commandID, bool active)
        {
            // Check if command ID is valid to be run.
            if (_commandID.ID != commandID) return;

            // Set active or deactive the UI interaction.
            if (_alphaGroup != null)
                _alphaGroup.alpha = active ? _originAlpha : _onDisableAlpha;
            else gameObject.SetActive(active);
            _isDisabled = !active;
        }

        #endregion
    }
}
