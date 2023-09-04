using JT.GameEvents;
using TMPro;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle reload timer for weapon info.
    /// </summary>
    [RequireComponent(typeof(RequireComponent))]
    public class UI_WeaponReloadTimer : MonoBehaviour
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private EntityID _elementID = null;

        [SerializeField]
        private TextMeshProUGUI _reloadTimerText = null;

        [Header("Properties")]
        [SerializeField]
        private bool _showTimerFloat = true;

        [Header("Game Events")]
        [SerializeField]
        private GameEventStringFloat _onReloadingDataChange = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events.
            _onReloadingDataChange.AddListener(ListenOnReloadingDataChange);
        }

        private void OnDestroy()
        {
            // Unsubscribe events.
            _onReloadingDataChange.RemoveListener(ListenOnReloadingDataChange);
        }

        private void Start()
        {
            // Always disable on start.
            gameObject.SetActive(false);
        }

        #endregion

        #region Main

        private void ListenOnReloadingDataChange(string elementID, float reloadTimer)
        {
            // Check element ID matches, if not then abort process.
            if (_elementID.ID != elementID) return;

            // Update reloading timer.
            string t;
            if (_showTimerFloat) t = $"{(int)reloadTimer}.{(int)(reloadTimer * 10f % 10f)}";
            else t = $"{Mathf.CeilToInt(reloadTimer)}";

            // Assign result text.
            _reloadTimerText.text = t;

            // Check reload begin or end.
            if (reloadTimer <= 0f) gameObject.SetActive(false);
            else if (!gameObject.activeSelf && reloadTimer > 0f) gameObject.SetActive(true);
        }

        #endregion
    }
}
