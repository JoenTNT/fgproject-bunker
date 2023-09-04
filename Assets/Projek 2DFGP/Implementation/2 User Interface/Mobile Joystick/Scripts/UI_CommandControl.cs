using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    [RequireComponent(typeof(RectTransform))]
    public class UI_CommandControl : MonoBehaviour, ICommand
    {
        #region Variable

        [Header("Properties")]
        [SerializeField]
        private UI_CommandControlData _data = null;

        [Header("Game Events")]
        [SerializeField]
        private GameEventString _onCommand = null;

        [SerializeField]
        private GameEventStringBool _setActiveCommand = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events
            _data.ButtonClickEvent.OnClick.AddListener(RunCommand);
            _setActiveCommand.AddListener(ListenSetActiveCommand);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _data.ButtonClickEvent.OnClick.RemoveListener(RunCommand);
            _setActiveCommand.RemoveListener(ListenSetActiveCommand);
        }

        #endregion

        #region ICommand

        public void RunCommand() => _onCommand.Invoke(_data.TargetID);

        #endregion

        #region Main

        private void ListenSetActiveCommand(string commandID, bool active)
        {
            // Check if command ID is valid to be run.
            if (_data.CommandID != commandID) return;

            // Set active or deactive the UI interaction.
            gameObject.SetActive(active);
        }

        #endregion
    }

    /// <summary>
    /// Handle data for UI Command Control.
    /// </summary>
    [System.Serializable]
    internal class UI_CommandControlData
    {
        #region Variable

        [SerializeField]
        private string _targetID = string.Empty;

        [SerializeField]
        private EntityID _commandID = null;

        [SerializeField]
        private UI_NonAlphaImageButton _button = null;

        #endregion

        #region Properties

        /// <summary>
        /// Target control ID.
        /// </summary>
        public string TargetID
        {
            get => _targetID;
            set => _targetID = value;
        }

        /// <summary>
        /// UI Command Control ID.
        /// </summary>
        public string CommandID => _commandID.ID;

        /// <summary>
        /// Button click event.
        /// </summary>
        public IClickEvent ButtonClickEvent => _button;

        #endregion
    }
}
