using System.Text;
using TMPro;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle weapon selection.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UI_AmmoInfo : MonoBehaviour
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private TextMeshProUGUI _ammoAmountText = null;

        #endregion

        #region Mono

        private void OnEnable() => _ammoAmountText.enabled = true;

        private void OnDisable() => _ammoAmountText.enabled = false;

        #endregion

        #region Main

        public void SetInfo(int ammo, int ammoInBag)
            => _ammoAmountText.SetText($"{ammo}/{ammoInBag}");

        #endregion
    }
}
