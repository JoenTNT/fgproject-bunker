using TMPro;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle reload timer for weapon info.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UI_ReloadTimeInfo : MonoBehaviour
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private TextMeshProUGUI _reloadTimerText = null;

        [Header("Properties")]
        [SerializeField]
        private bool _showTimerFloat = true;

        #endregion

        #region Mono

        // Always disable on start.
        private void Start() => gameObject.SetActive(false);

        #endregion

        #region Main

        public void SetInfo(float reloadTimer)
        {
            // Update reloading timer.
            string t;
            if (_showTimerFloat) t = $"{(int)reloadTimer}.{(int)(reloadTimer * 10f % 10f)}";
            else t = $"{Mathf.CeilToInt(reloadTimer)}";

            // Assign result text.
            _reloadTimerText.SetText(t);

            // Check reload begin or end.
            if (reloadTimer <= 0f) gameObject.SetActive(false);
            else if (!gameObject.activeSelf && reloadTimer > 0f) gameObject.SetActive(true);
        }

        #endregion
    }
}
