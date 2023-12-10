using UnityEngine;
using UnityEngine.Events;

namespace JT.FGP
{
    /// <summary>
    /// To open notification dialog on the center of the screen.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UI_NotificationDialog : MonoBehaviour, IOpenCloseAction
    {
        #region Variables

        [Header("Game Events")]
        [SerializeField]
        private UnityEvent<string> _onDialogOpened = new UnityEvent<string>();

        [SerializeField]
        private UnityEvent<string> _onDialogClosed = new UnityEvent<string>();

        // Runtime variable data.
        private bool _isOpened = false;

        #endregion

        #region Properties

        /// <summary>
        /// Called after notification dialog opened.
        /// </summary>
        public UnityEvent<string> OnDialogOpened => _onDialogOpened;

        /// <summary>
        /// Called after notification dialog closed.
        /// </summary>
        public UnityEvent<string> OnDialogClosed => _onDialogClosed;

        #endregion

        #region IOpenCloseAction

        public bool IsOpened => _isOpened;

        public void Close()
        {
            // Ignore when already closed.
            if (!_isOpened) return;

            // TODO: Close dialog notif.
            _isOpened = false;
        }

        public void Open()
        {
            // Ignore when already opened.
            if (_isOpened) return;

            // TODO: Open dialog notif.
            _isOpened = true;
        }

        #endregion
    }
}
