using JT.GameEvents;
using TMPro;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Controller notification display.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UI_InteractionNotifier : MonoBehaviour, ITargetID<string>
    {
        #region Variable

        [Header("Dependency")]
        [SerializeField]
        private string _targetID = string.Empty;

        [Header("Requirements")]
        [SerializeField]
        private TextMeshProUGUI _hintText = null;

        [Header("Properties")]
        /// <summary>
        /// Hint text display contents.
        /// </summary>
        /// <see cref="InteractionType"/>
        [SerializeField, Tooltip("\"Interaction Type\" in string as a hint.")]
        private string[] _prefix = { "Interact with", "Collect", "Talk to" };

        [SerializeField]
        private bool _disableOnStart = true;

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

        private void Start() => _hintText.enabled = !_disableOnStart;
#if UNITY_EDITOR
        private void OnValidate()
        {
            var itEnum = System.Enum.GetValues(typeof(InteractionType));
            if (_prefix.Length == itEnum.Length - 1) return;

            var old = _prefix;
            _prefix = new string[itEnum.Length - 1];
            System.Array.Copy(old, _prefix,
                old.Length < itEnum.Length - 1 ? old.Length : itEnum.Length - 1);
        }
#endif
        #endregion

        #region ITargetID

        public string TargetID
        {
            get => _targetID;
            set => _targetID = value;
        }

        #endregion

        #region Main

        private void ListenOnNotifyInteractionHint(string whoInteract, string interactWith, int type)
        {
            if (_targetID != whoInteract) return;

            // Activate object if should be active.
            _hintText.enabled = type >= 0 && type < _prefix.Length;

            // Set hint text if enabled.
            if (!_hintText.enabled) return;
            _hintText.text = $"{_prefix[type]} {interactWith}";
        }

        #endregion
    }
}
