using JT.GameEvents;
using System.Collections.Generic;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle taking item when interact.
    /// </summary>
    public class CollectInteract : InteractableComponent, ICollectable<IReadOnlyList<RuntimeInfoItem>>,
        IRequiredReset
    {
        #region Variable

        [Header("Requirements")]
        [SerializeField]
        private SpriteRenderer _renderer = null;

        [SerializeField]
        private List<RuntimeInfoItem> _items = new();

        [Header("Properties")]
        [SerializeField, Min(-1)]
        private int _initialAmount = -1; // Minus 1 means infinite.

        [SerializeField]
        private bool _destroyOnCollect = false;

        [Header("Game Events")]
        [SerializeField]
        private GameEventStringUnityObject _onCollectItem = null;

        // Runtime variable data.
        private GameObjectPoolManager _poolManager = null;
        private Queue<object> _poolRef = null;
        private RuntimeInfoItem _tempItem;
        private int _itemLeftover = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Initial amount of collectable dropped item, along with leftover.
        /// </summary>
        public int InitialAmount
        {
            get => _initialAmount;
            set
            {
                _initialAmount = value;
                if (_itemLeftover > _initialAmount)
                    _itemLeftover = _initialAmount;
            }
        }

        /// <summary>
        /// Leftover collects.
        /// </summary>
        public int Leftover
        {
            get => _itemLeftover;
            set
            {
                _itemLeftover = value;
                if (_itemLeftover > _initialAmount)
                    _itemLeftover = _initialAmount;
            }
        }

        #endregion

        #region Mono

        // Receive pool manager reference before executing everything else.
        private void Start() => _poolManager = GameObjectPoolManager.Instance;

        private void OnEnable()
        {
            // Initialize data.
            if (_items.Count <= 0)
            {
                // Disable if there's no item property.
                gameObject.SetActive(false);
                return;
            }

            // Set icon with first priority.
            ID = _items[0].ItemPreset.Name;
            _renderer.sprite = _items[0].ItemPreset.Icon;

            // Always set item leftover to 1 if leftover not assigned.
            if (_itemLeftover == 0) _itemLeftover = 1;

            // Start handle initiation of runtime data.
            HandleRuntimeDataCreation();
        }

        #region IRequiredReset

        /// <summary>
        /// This reset used to send all items back to pool and disable collect interact.
        /// Not for setting back to default item values.
        /// </summary>
        public void Reset()
        {
            // Send all cache to pool.
            int itemCount = _items.Count;
            for (int i = 0; i < itemCount; i++)
            {
                // Skip if there's no cache.
                _tempItem = _items[i];
                if (_tempItem.CacheInfo == null)
                    continue;

                // Check which pool used.
                switch (_tempItem.ItemPreset)
                {
                    case RangedWeaponItemSO rw:
                        _poolRef = _poolManager.GetNonMonoPool(RuntimeAmmoInfo.FID);
                        _poolRef.Enqueue(_items[i].CacheInfo);
                        break;
                }
            }

            // Empty up items.
            _items.Clear();
            gameObject.SetActive(false);
        }

        #endregion

        #endregion

        #region InteractableComponent

        public override InteractionType Type => InteractionType.PickUpItem;

        public override bool Interact(string entity)
        {
            // Collect item event.
            _onCollectItem.Invoke(entity, this);
            return true;
        }

        #endregion

        #region ICollectable

        public IReadOnlyList<RuntimeInfoItem> Collect() => _items;

        #endregion

        #region Main

        /// <summary>
        /// Adding runtime item cache on collectable item.
        /// Usually used when an entity dropped an item.
        /// </summary>
        /// <param name="item">Runtime item and cache</param>
        public void RegisterItem(RuntimeInfoItem item)
        {
            // Check if runtime data exists, if not then create one.
            if (item.ItemPreset != null && item.CacheInfo == null)
            {
                // Get cache from pool.
                var poolManager = GameObjectPoolManager.Instance;

                // Check which pool used.
                switch (item.ItemPreset)
                {
                    case RangedWeaponItemSO rw:
                        _poolRef = poolManager.GetNonMonoPool(RuntimeAmmoInfo.FID);
                        if (_poolRef.Count > 0)
                        {
                            var rai = (RuntimeAmmoInfo)_poolRef.Dequeue();
                            rw.AmmoInfoPreset.CopyValue(rai);
                            item.CacheInfo = rai;
                            break;
                        }
                        item.CacheInfo = rw.AmmoInfoPreset.CreateRuntimeObject();
                        break;
                }
            }

            // Add cache.
            _items.Add(item);
        }

        internal void TakeOne()
        {
            // Collect an item.
            if (_itemLeftover != -1) _itemLeftover--;

            // Disable or destroy item when collected.
            if (_itemLeftover != 0) return;

            // Check destroy game object.
            if (_destroyOnCollect) Destroy(gameObject);
            else Reset();
        }

        private void HandleRuntimeDataCreation()
        {
            // Initialize runtime data if exists in each item data.
            int itemsLen = _items.Count;
            if (itemsLen <= 0) return; // Do not run if there's no item.

            // Check each item.
            for (int i = 0; i < itemsLen; i++)
            {
                // Create all runtime data on start.
                _tempItem = _items[i];
                if (_tempItem.ItemPreset == null)
                {
                    _items.RemoveAt(i);
                    itemsLen--;
                    i--;
                    continue;
                }

                // Check which type of item.
                switch (_tempItem.ItemPreset)
                {
                    case RangedWeaponItemSO rw:
                        _poolRef = _poolManager?.GetNonMonoPool(RuntimeAmmoInfo.FID);
                        if (_poolRef == null || _poolRef.Count <= 0)
                        {
                            _tempItem.CacheInfo = rw.AmmoInfoPreset.CreateRuntimeObject();
                            break;
                        }
                        var ammoInfo = (RuntimeAmmoInfo)_poolRef.Dequeue();
                        rw.AmmoInfoPreset.CopyValue(ammoInfo);
                        _tempItem.CacheInfo = ammoInfo;
                        break;

                    case MeleeWeaponItemSO mw:
                        // TODO: Get and copy melee weapon runtime item data.
                        break;
                }
            }
        }

        #endregion
    }
}
