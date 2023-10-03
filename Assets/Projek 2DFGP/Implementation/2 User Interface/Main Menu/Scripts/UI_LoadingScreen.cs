using JT.GameEvents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JT.FGP
{
    /// <summary>
    /// Handle loading screen of the game.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UI_LoadingScreen : MonoBehaviour
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private Slider _loadingBar = null;

        [SerializeField]
        private TextMeshProUGUI _loadingText = null;

        [Header("Properties")]
        [SerializeField]
        private bool _disableOnStart = true;

        [Header("Game Events")]
        [Tooltip("Event called when loading screen has been shown.")]
        [SerializeField]
        private GameEventNoParam _onStartLoadingScreen = null;

        [Tooltip("Event called when loading screen has been closed.")]
        [SerializeField]
        private GameEventNoParam _onEndLoadingScreen = null;

        [Tooltip("Event called at runtime to set loading text.")]
        [SerializeField]
        private GameEventString _onSetLoadingScreenText = null;

        [Tooltip("Event called at runtime to set loading value in percentage.")]
        [SerializeField]
        private GameEventFloat _onSetLoadingValuePercent = null;

        #endregion

        #region Properties

        public RectTransform rectTransform => (RectTransform)transform;

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events.
            _onStartLoadingScreen.AddListener(ListenOnStartLoadingScreen);
            _onEndLoadingScreen.AddListener(ListenOnEndLoadingScreen);
            _onSetLoadingScreenText.AddListener(ListenOnSetLoadingScreenText);
            _onSetLoadingValuePercent.AddListener(ListenOnSetLoadingValuePercent);
        }

        private void OnDestroy()
        {
            // Unsubscribe events.
            _onStartLoadingScreen.RemoveListener(ListenOnStartLoadingScreen);
            _onEndLoadingScreen.RemoveListener(ListenOnEndLoadingScreen);
            _onSetLoadingScreenText.RemoveListener(ListenOnSetLoadingScreenText);
            _onSetLoadingValuePercent.RemoveListener(ListenOnSetLoadingValuePercent);
        }

        private void Start()
        {
            if (_disableOnStart)
                gameObject.SetActive(false);
        }

        #endregion

        #region Main

        private void ListenOnStartLoadingScreen()
        {
            // Close loading screen.
            gameObject.SetActive(true);
        }

        private void ListenOnEndLoadingScreen()
        {
            // Open loading screen.
            gameObject.SetActive(false);
        }

        private void ListenOnSetLoadingScreenText(string loadingText)
        {
            // Set loading text.
            _loadingText.text = loadingText;
        }

        private void ListenOnSetLoadingValuePercent(float percentValue)
        {
            // Set loading value.
            _loadingBar.value = percentValue;
        }

        #endregion
    }
}
