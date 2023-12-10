using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// A pair between item preset and runtime information.
    /// </summary>
    [System.Serializable]
    public struct RuntimeInfoItem : IRequiredReset
    {
        #region Variables

        [SerializeField]
        private ItemSO _itemPreset;

        [SerializeField, Min(-1)]
        private int _defaultAmount;

        // Runtime variable data.
        private RuntimeInfo _cacheInfo;
        private int _leftover;

        #endregion

        #region Properties

        /// <summary>
        /// Item preset received.
        /// </summary>
        public ItemSO ItemPreset
        {
            get => _itemPreset;
            set => _itemPreset = value;
        }

        /// <summary>
        /// Cache information received.
        /// </summary>
        public RuntimeInfo CacheInfo
        {
            get => _cacheInfo;
            set => _cacheInfo = value;
        }

        /// <summary>
        /// How much leftover of this item type.
        /// </summary>
        public int Leftover
        {
            get => _leftover;
            set => _leftover = value;
        }

        #endregion

        #region IRequiredReset

        public void Reset()
        {
            // Reset runtime information if item exists.
            if (_itemPreset != null)
            {
                switch (_itemPreset)
                {
                    case RangedWeaponItemSO rw when _cacheInfo == null:
                        _cacheInfo = rw.AmmoInfoPreset.CreateRuntimeObject();
                        break;

                    case RangedWeaponItemSO rw when _cacheInfo != null && _cacheInfo is RuntimeAmmoInfo:
                        rw.AmmoInfoPreset.CopyValue((RuntimeAmmoInfo)_cacheInfo);
                        break;

                    case MeleeWeaponItemSO mw:
                        // TODO: Melle weapon checker.
                        break;
                }
            }

            // Reset leftover amount to default value.
            _leftover = _defaultAmount;
        }

        #endregion
    }
}