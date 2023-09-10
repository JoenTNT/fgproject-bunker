using TMPro;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle weapon selection.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UI_WeaponGunInfo : MonoBehaviour
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private TextMeshProUGUI _ammoAmountText = null;

        #endregion

        #region Mono

        private void OnEnable() => _ammoAmountText.gameObject.SetActive(true);

        private void OnDisable() => _ammoAmountText.gameObject.SetActive(false);

        #endregion

        #region Main

        public void SetInfo(int ammo, int ammoInBag)
        {
            // Update data immediately.
            _ammoAmountText.text = $"{ammo}/{ammoInBag}";
        }

        #endregion
    }
}
