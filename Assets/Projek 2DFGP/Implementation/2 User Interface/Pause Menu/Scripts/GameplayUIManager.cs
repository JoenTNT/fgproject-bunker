using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle user interface in gameplay.
    /// </summary>
    public sealed class GameplayUIManager : MonoBehaviour
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private Canvas _gameplayInfo = null;

        [SerializeField]
        private Canvas _mobileController = null;

        [SerializeField]
        private Canvas _pauseMenu = null;

        [Header("Game Events")]
        [SerializeField]
        private GameEventNoParam _openPauseMenuCallback = null;

        [SerializeField]
        private GameEventNoParam _closePauseMenuCallback = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Always disable pause menu.
            _pauseMenu.gameObject.SetActive(false);

            // Subcribe events.
            _openPauseMenuCallback.AddListener(ListenOpenPauseMenuCallback);
            _closePauseMenuCallback.AddListener(ListenClosePauseMenuCallback);
        }

        private void OnDestroy()
        {
            // Unsubcribe events.
            _openPauseMenuCallback.RemoveListener(ListenOpenPauseMenuCallback);
            _closePauseMenuCallback.RemoveListener(ListenClosePauseMenuCallback);
        }

        #endregion

        #region Main

        private void ListenOpenPauseMenuCallback()
        {
            Time.timeScale = 0f;
            _pauseMenu.gameObject.SetActive(true);
        }

        private void ListenClosePauseMenuCallback()
        {
            Time.timeScale = 1f;
            _pauseMenu.gameObject.SetActive(false);
        }

        #endregion
    }
}
