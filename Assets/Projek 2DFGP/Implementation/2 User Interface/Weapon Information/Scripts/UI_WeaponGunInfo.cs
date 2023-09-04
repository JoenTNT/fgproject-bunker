using JT.GameEvents;
using TMPro;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle weapon selection.
    /// </summary>
    [RequireComponent(typeof(UI_WeaponInfoManager))]
    public class UI_WeaponGunInfo : MonoBehaviour
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private TextMeshProUGUI _ammoAmountText = null;

        [Header("Game Events")]
        [SerializeField]
        private GameEventStringTwoInt _onAmmoDataChange = null;

        // Runtime variable data.
        private UI_WeaponInfoManager _manager = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Get manager component.
            TryGetComponent(out _manager);

            // Subscribe events.
            _onAmmoDataChange.AddListener(ListenOnAmmoDataChange);
        }

        private void OnDestroy()
        {
            // Unsubscribe events.
            _onAmmoDataChange.RemoveListener(ListenOnAmmoDataChange);
        }

        private void OnEnable() => _ammoAmountText.gameObject.SetActive(true);

        private void OnDisable() => _ammoAmountText.gameObject.SetActive(false);

        #endregion

        #region Main

        private void ListenOnAmmoDataChange(string elementID, int ammo, int ammoInBag)
        {
            // Check element ID matches, if not then abort process.
            if (_manager.ElementID != elementID) return;

            // Set information.
            SetInfo(ammo, ammoInBag);
        }

        public void SetInfo(int ammo, int ammoInBag)
        {
            // Update data immediately.
            _ammoAmountText.text = $"{ammo}/{ammoInBag}";
        }

        #endregion
    }
}
