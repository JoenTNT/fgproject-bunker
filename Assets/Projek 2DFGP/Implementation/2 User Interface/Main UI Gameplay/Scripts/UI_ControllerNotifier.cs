using JT.GameEvents;
using TMPro;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Controller notification display.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UI_ControllerNotifier : MonoBehaviour
    {
        #region Variable

        [Header("Properties")]
        [SerializeField]
        private UI_ControllerNotifierData _data = null;

        [Header("Game Events")]
        [SerializeField]
        private GameEventStringInt _onNotifyInteractionHint = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events
            _onNotifyInteractionHint.AddListener(ListenOnNotifyInteractionHint);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _onNotifyInteractionHint.RemoveListener(ListenOnNotifyInteractionHint);
        }

        #endregion

        #region Main

        private void ListenOnNotifyInteractionHint(string byEntityID, int type)
        {
            if (_data.TargetID != byEntityID) return;

            gameObject.SetActive(_data.SetHintText(type));
        }

        #endregion
    }

    /// <summary>
    /// Handle data for UI controller notifier.
    /// </summary>
    [System.Serializable]
    internal class UI_ControllerNotifierData : ITargetID<string>
    {
        #region Variable

        [SerializeField]
        private string _targetID = string.Empty;

        [SerializeField]
        private TextMeshProUGUI _hintText = null;

        /// <summary>
        /// Hint text display contents.
        /// </summary>
        /// <see cref="InteractionType"/>
        [SerializeField, Tooltip("\"Interaction Type\" in string as a hint.")]
        private string[] _hintTextContents = { "to Interact", "to Pickup", "to Talk" };

        #endregion

        #region Properties

        /// <summary>
        /// How many hint content available.
        /// </summary>
        public int HintContentCount => _hintTextContents.Length;

        #endregion

        #region Main

        public bool SetHintText(int index)
        {
            if (index < 0 || index >= _hintTextContents.Length) return false;

            _hintText.text = _hintTextContents[index];
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
