using JT.GameEvents;
using System.Collections.Generic;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Used to manage physical tool slots in which character holds.
    /// </summary>
    public sealed class PlayerItemSlotManager : MonoBehaviour, IInjectDependency<PlayerEntityData>
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private EntityID _ownerID = null;

        [SerializeField]
        private RangedWeapon _rangedWeapon = null;

        [SerializeField]
        private MeleeWeapon _meleeWeapon = null;

        [SerializeField]
        private DropPhysicalItemFunc _dropFunc = null;

        [Header("Properties")]
        [SerializeField]
        private string _primarySlotKey = "Primary";

        [SerializeField]
        private string _dropButtonKey = "Drop";

        [SerializeField]
        private string _collectableItemPoolKey = "Collectable Item";

        [Header("Game Events")]
        [SerializeField]
        private GameEventStringUnityObject _onCollectItem = null;

        [SerializeField]
        private GameEventStringUnityObject _injectUIInfoCallback = null;

        [SerializeField]
        private GameEventTwoString _onToolSlotCommand = null;

        [SerializeField]
        private GameEventStringBool _setActiveDropButtonCommand = null;

        [SerializeField]
        private GameEventString _onDropPrimaryItem = null;

        // Runtime variable data.
        private GameObjectPool _collectableItemPoolRef = null;
        private PlayerEntityData _playerData = null;
        private PhysicalItemSlot _primaryToolSlot = null;
        private CollectInteract _tempCollected = null;
        private Dictionary<string, PhysicalItemSlot> _registeredToolSlots = null;
        private PhysicalItemSlot _tempToolSlot = null;
        private ConsumableData[] _tempConsumable = null;
        private ConsumableData _tempConsumeData;

        #endregion

        #region Properties

        /// <summary>
        /// Receive collectable item pool including checker.
        /// </summary>
        private GameObjectPool CollectableItemPool
        {
            get
            {
                if (_collectableItemPoolRef != null) return _collectableItemPoolRef;
                return _collectableItemPoolRef = GameObjectPoolManager.Instance
                    .GetGameObjPool(_collectableItemPoolKey);
            }
        }

        #endregion

        #region Mono

        private void Awake()
        {
            // Initialize data.
            _registeredToolSlots = new();

            // Subscribe events.
            _onCollectItem.AddListener(ListenOnCollectItem);
            _injectUIInfoCallback.AddListener(ListenInjectUIInfoCallback);
            _onToolSlotCommand.AddListener(ListenOnToolSlotCommand);
            _onDropPrimaryItem.AddListener(ListenOnDropPrimaryItem);
        }

        private void ListenOnDropPrimaryItem(string ownerID)
        {
            // Validate ID.
            if (ownerID != _ownerID.ID) return;

            // Check primary item tool slot is filled, if not ignore command.
            _primaryToolSlot = _registeredToolSlots[_primarySlotKey];
            if (_primaryToolSlot.ItemHold == null) return;

            // Receive item from pool, then register to empty collectable item.
            CollectableItemPool.GetObject().TryGetComponent(out _tempCollected);
            _tempCollected.Reset();
            _tempCollected.RegisterItem(new RuntimeInfoItem
            {
                ItemPreset = _primaryToolSlot.ItemHold,
                CacheInfo = _primaryToolSlot.RuntimeInfo,
                Leftover = 1,
            });
            _tempCollected.gameObject.SetActive(true);

            // Empty the slot.
            _primaryToolSlot.ReplaceSlot(null, null, 0);

            // Drop item now.
            _dropFunc.DropObject(_tempCollected.transform);
            
            // Call event diable drop button after dropping primary item.
            _setActiveDropButtonCommand.Invoke(_dropButtonKey, false, this);
        }

        private void OnDestroy()
        {
            // Unsubscribe events.
            _onCollectItem.RemoveListener(ListenOnCollectItem);
            _injectUIInfoCallback.RemoveListener(ListenInjectUIInfoCallback);
            _onToolSlotCommand.RemoveListener(ListenOnToolSlotCommand);
            _onDropPrimaryItem.RemoveListener(ListenOnDropPrimaryItem);
        }

        #endregion

        #region IInjectDependency

        public void Inject(PlayerEntityData instance) => _playerData = instance;

        #endregion

        #region Main

        private void ListenOnCollectItem(string id, Object itemCollected)
        {
            // Validate ID.
            if (_playerData.ID != id) return;
            if (itemCollected is not CollectInteract) return;

            // Collected items checker.
            _tempCollected = (CollectInteract)itemCollected;
            var items = _tempCollected.Collect();
            int len = items.Count;
            bool success = false;
            for (int i = 0; i < len; i++)
            {
                switch (items[i].ItemPreset)
                {
                    case ConsumableItemSO con:
                        _tempConsumable = con.ConsumerGain;
                        int lenJ = _tempConsumable.Length;
                        for (int j = 0; j < lenJ; j++)
                        {
                            _tempConsumeData = _tempConsumable[j];
                            switch (_tempConsumeData.Type)
                            {
                                case ConsumableItemType.Healing:
                                    _playerData.PhysicalStats.Heal(_tempConsumeData.GainValue);
                                    success = true;
                                    break;

                                case ConsumableItemType.Ammunition:
                                    // TODO: Collect ammunition to primary used weapon.
                                    break;
                            }
                        }
                        break;

                    case RangedWeaponItemSO rw:
                        // Check primary slot tool that already has item hold in slot.
                        _primaryToolSlot = _registeredToolSlots[_primarySlotKey];
                        if (_primaryToolSlot.ItemHold == null)
                        {
                            AssignWeaponItem(rw, _primaryToolSlot);
                            success = true;
                            break;
                        }

                        // Check other empty slots.
                        _tempToolSlot = null;
                        foreach (var rt in _registeredToolSlots)
                        {
                            if (rt.Value.ItemHold != null) continue;
                            _tempToolSlot = rt.Value;
                            break;
                        }
                        if (_tempToolSlot != null)
                        {
                            AssignWeaponItem(rw, _tempToolSlot);
                            success = true;
                        }
                        break;

                    case MeleeWeaponItemSO mw:
                        // TODO: Check which slot must be assigned, then assign melee weapon.
                        break;
                }
            }

            // Check collected item validation.
            if (success) _tempCollected.TakeOne();
        }

        private void ListenInjectUIInfoCallback(string ownerID, Object uiObj)
        {
            // Validate informations.
            if (uiObj is not UI_ToolSlot) return;
            if (ownerID != _ownerID.ID) return;

            // Create physical tool slot.
            var newToolSlot = new PhysicalItemSlot((UI_ToolSlot)uiObj);
            _registeredToolSlots[newToolSlot.ToolSlotID] = newToolSlot;

            // Check primary slot drop button activation.
            if (newToolSlot.ToolSlotID != _primarySlotKey) return;
            _setActiveDropButtonCommand.Invoke(_dropButtonKey, newToolSlot.ItemHold != null, this);
        }

        private void ListenOnToolSlotCommand(string ownerID, string toolSlotID)
        {
            // Validate identifier.
            if (ownerID != _ownerID.ID) return;
            if (!_registeredToolSlots.ContainsKey(toolSlotID)) return;

            // Check for consumable, because it will be consumed immediately.
            _tempToolSlot = _registeredToolSlots[toolSlotID];
            switch (_tempToolSlot.ItemHold)
            {
                case ConsumableItemSO consumable:
                    _tempConsumable = consumable.ConsumerGain;
                    int len = _tempConsumable.Length;
                    for (int i = 0; i < len; i++)
                    {
                        _tempConsumeData = _tempConsumable[i];
                        switch (_tempConsumeData.Type)
                        {
                            case ConsumableItemType.Healing:
                                _playerData.PhysicalStats.Heal(_tempConsumeData.GainValue);
                                break;

                            case ConsumableItemType.Ammunition:
                                // TODO: Fill the ammunition one of the ammunition cache.
                                break;
                        }
                    }
                    return;
            }

            // Get primary tool used slot with old item data.
            _primaryToolSlot = _registeredToolSlots[_primarySlotKey];

            // Disable old primary tool.
            switch (_primaryToolSlot.ItemHold)
            {
                case RangedWeaponItemSO rw:
                    _rangedWeapon.gameObject.SetActive(false);
                    break;

                case MeleeWeaponItemSO mw:
                    _meleeWeapon.gameObject.SetActive(false);
                    break;
            }

            // Switch tool slot with new one.
            _primaryToolSlot.SwapValue(_tempToolSlot);

            // Enable tool slot used.
            switch (_primaryToolSlot.ItemHold)
            {
                case RangedWeaponItemSO rw:
                    _rangedWeapon.gameObject.SetActive(true);
                    var runtimeAmmoInfo = (RuntimeAmmoInfo)_primaryToolSlot.RuntimeInfo;
                    _rangedWeapon.ChangeWeaponBehaviour(rw, ref runtimeAmmoInfo);
                    _playerData.Weapon = _rangedWeapon.WeaponOwnerAdapter;

                    // Call event diable drop button after dropping primary item.
                    _setActiveDropButtonCommand.Invoke(_dropButtonKey, true, this);
                    break;

                case MeleeWeaponItemSO mw:
                    _meleeWeapon.gameObject.SetActive(true);
                    _playerData.Weapon = _meleeWeapon.WeaponOwnerAdapter;

                    // Call event diable drop button after dropping primary item.
                    _setActiveDropButtonCommand.Invoke(_dropButtonKey, true, this);
                    break;

                case null:
                    _playerData.Weapon = null;

                    // Call event diable drop button after dropping primary item.
                    _setActiveDropButtonCommand.Invoke(_dropButtonKey, false, this);
                    break;
            }
        }

        private void AssignWeaponItem(RangedWeaponItemSO item, PhysicalItemSlot targetSlot)
        {
            // Assign or replace new cache on physical weapon.
            var runtimeAmmoInfo = (RuntimeAmmoInfo)targetSlot.RuntimeInfo;

            // Assign or replace new cache to physical tool slot.
            targetSlot.ReplaceSlot(item, runtimeAmmoInfo, 1);

            // Check primary slot activation.
            if (_primaryToolSlot != targetSlot) return;

            // Assign or replace new cache on physical weapon.
            _rangedWeapon.ChangeWeaponBehaviour(item, ref runtimeAmmoInfo);

            // Check enable or disable physical ranged weapon.
            if (_primaryToolSlot.ItemHold == null)
            {
                _playerData.Weapon = null;
                _rangedWeapon.gameObject.SetActive(false);
                return;
            }

            // Else then activate ranged weapon.
            _playerData.Weapon = _rangedWeapon.WeaponOwnerAdapter;
            _rangedWeapon.gameObject.SetActive(true);
            _setActiveDropButtonCommand.Invoke(_dropButtonKey, true, this);
        }

        private void AssignWeaponItem(MeleeWeaponItemSO item, PhysicalItemSlot targetSlot)
        {
            // TODO: Assign weapon preset.
            //_meleeWeapon.gameObject.SetActive(true);
            //_playerData.Weapon = _meleeWeapon.WeaponOwnerAdapter;
        }

        #endregion
    }
}
