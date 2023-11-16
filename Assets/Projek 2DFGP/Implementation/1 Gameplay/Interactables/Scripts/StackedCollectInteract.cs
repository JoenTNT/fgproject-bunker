using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle taking item when interact.
    /// </summary>
    public sealed class StackedCollectInteract : InteractableComponent, IRequiredReset
    {
        #region Variable

        [Header("Requirements")]
        //[SerializeField]
        //private AbstractItemPresetSO[] _itemPresets = null;

        [Header("Properties")]
        [SerializeField, Min(-1)]
        private int _itemAmount = -1; // -1 means Infiite.

        [SerializeField]
        private bool _disableOnAwake = false;

        [SerializeField]
        private bool _destroyOnCollect = false;

        [Header("Game Events")]
        [SerializeField]
        private GameEventStringObject _onCollectItem = null;

        // Runtime variable data.
        private ItemSO[] _runtimeData = null;
        private int _itemLeftover = 0;

        #endregion

        #region Mono

        private void Awake()
        {
            //// Initialize data.
            //_runtimeData = new ItemSO[_itemPresets.Length];
            //int len = _itemPresets.Length;
            //for (int i = 0; i < len; i++)
            //    _runtimeData[i] = _itemPresets[i].CreateRuntimeObject();
            //Reset();

            //// Check disable on start.
            //if (_disableOnAwake) gameObject.SetActive(false);
        }

        #region IRequiredReset

        public void Reset()
        {
            // Reinitialize data.
            _itemLeftover = _itemAmount;
        }

        #endregion

        #endregion

        #region InteractableComponent

        public override InteractionType Type => InteractionType.PickUpItem;

        public override bool Interact(string entity)
        {
            // Collect item event.
            int len = _runtimeData.Length;
            for (int i = 0; i < len; i++)
                _onCollectItem.Invoke(entity, _runtimeData[i]);

            // Collect an item.
            if (_itemLeftover != -1) _itemLeftover--;

            // Disable or destroy item when collected.
            if (_itemLeftover == 0)
            {
                if (_destroyOnCollect) Destroy(gameObject);
                else gameObject.SetActive(false);
            }
            return true;
        }

        #endregion
    }
}
