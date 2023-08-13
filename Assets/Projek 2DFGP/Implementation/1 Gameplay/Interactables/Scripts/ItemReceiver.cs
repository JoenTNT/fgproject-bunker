using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Entity gets the item.
    /// </summary>
    public class ItemReceiver : InteractableObject
    {
        #region Variable

        [Header("Properties")]
        [SerializeField]
        private string _itemName = string.Empty;

        [SerializeField]
        private int _amount = 0;

        [Header("Game Events")]
        [SerializeField]
        private GameEventTwoStringInt _getItemCallback = null;

        #endregion

        #region InteractableObject

        public override InteractionType InteractType => InteractionType.PickUpItem;

        public override void OnProcess(string entityID)
        {
#if UNITY_EDITOR
            Debug.Log($"Collected: {_itemName} - {_amount}");
#endif
            _getItemCallback.Invoke(entityID, _itemName, _amount);
            Finish();
        }

        #endregion
    }
}
