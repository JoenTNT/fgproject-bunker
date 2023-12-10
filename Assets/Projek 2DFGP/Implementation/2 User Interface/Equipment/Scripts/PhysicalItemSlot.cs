namespace JT.FGP
{
    /// <summary>
    /// Used as a bridge to connect physical and UI information.
    /// </summary>
    public sealed class PhysicalItemSlot : ISwapValue<PhysicalItemSlot>
    {
        #region Variables

        // Runtime variable data.
        private UI_ToolSlot _targetToolSlot = null;
        private RuntimeInfoItem _currentItemInfo;

        #endregion

        #region Properties

        /// <summary>
        /// Structure of current item filled slot.
        /// </summary>
        public RuntimeInfoItem CurrentItemSlot => _currentItemInfo;

        /// <summary>
        /// Item that is hold in the slot.
        /// </summary>
        public ItemSO ItemHold => _currentItemInfo.ItemPreset;

        /// <summary>
        /// Ammunition information.
        /// </summary>
        public RuntimeInfo RuntimeInfo => _currentItemInfo.CacheInfo;

        /// <summary>
        /// This tool slot ID to connect with owner's physical slot.
        /// </summary>
        public string ToolSlotID => _targetToolSlot.ToolSlotID;

        #endregion

        #region ISwapValue

        /// <summary>
        /// Swap informations, not injected UI.
        /// </summary>
        /// <param name="target">Target physical tool slot</param>
        public void SwapValue(PhysicalItemSlot target)
        {
            // Swap item in slot.
            var tempItem = target._currentItemInfo;
            target._currentItemInfo = _currentItemInfo;
            _currentItemInfo = tempItem;

            // Sync both information.
            target.SyncInfoUI();
            SyncInfoUI();
        }

        #endregion

        #region Main

        /// <summary>
        /// Replace item information in slot.
        /// </summary>
        /// <param name="newItem">The new item structure</param>
        public void ReplaceSlot(RuntimeInfoItem newItem)
        {
            // Send the old one to pool.
            var poolManager = GameObjectPoolManager.Instance;
            switch (_currentItemInfo.CacheInfo)
            {
                // Send back the ammo info to pool target.
                case RuntimeAmmoInfo ammo:
                    poolManager.GetNonMonoPool(RuntimeAmmoInfo.FID).Enqueue(ammo);
                    break;
            }

            // Replace runtime info.
            _currentItemInfo = newItem;
            switch (_currentItemInfo.ItemPreset)
            {
                // Send back the ammo info to pool target.
                case RangedWeaponItemSO rw:
                    var ammoPool = poolManager.GetNonMonoPool(RuntimeAmmoInfo.FID);
                    if (ammoPool.Count <= 0)
                    {
                        _currentItemInfo.CacheInfo = rw.AmmoInfoPreset.CreateRuntimeObject();
                        break;
                    }
                    var ammoInfo = (RuntimeAmmoInfo)ammoPool.Dequeue();
                    rw.AmmoInfoPreset.CopyValue(ammoInfo);
                    _currentItemInfo.CacheInfo = ammoInfo;
                    break;
            }

            SyncInfoUI();
        }

        /// <summary>
        /// Replace item information in slot, all must be filled.
        /// </summary>
        /// <param name="newHold">Item hold in this slot as a replacement</param>
        /// <param name="newInfo">Runtime cache information</param>
        /// <param name="newAmount">Amount of item hold</param>
        public void ReplaceSlot(ItemSO newHold, RuntimeInfo newInfo, int newAmount)
            => ReplaceSlot(new RuntimeInfoItem { ItemPreset = newHold, CacheInfo = newInfo,
                Leftover = newAmount});

        private void SyncInfoUI()
        {
            // Select icon of UI slot.
            _targetToolSlot.SetIcon(_currentItemInfo.ItemPreset?.Icon);

            // TODO: Filler effect.
            _targetToolSlot.SetFillPercentage(1f);
        }

        #endregion

        #region Constructor

        // Ignore empty constructor.
        private PhysicalItemSlot() { }

        public PhysicalItemSlot(UI_ToolSlot uiToolSlot)
        {
            // Set target UI.
            _targetToolSlot = uiToolSlot;

            // Initialize item info.
            _currentItemInfo = new RuntimeInfoItem();
        }

        #endregion
    }
}

