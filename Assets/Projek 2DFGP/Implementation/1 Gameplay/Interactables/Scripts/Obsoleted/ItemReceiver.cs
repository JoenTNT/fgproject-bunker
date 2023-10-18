using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Entity gets the item.
    /// </summary>
    [System.Obsolete]
    public class ItemReceiver : InteractableObject
    {
        #region Variable

        [Header("Properties")]
        [SerializeField]
        private string _itemName = string.Empty;

        [SerializeField]
        private int _amount = 0;

        [SerializeField]
        private bool _hideOnCollected = false;

        [Header("Optional")]
        [SerializeField]
        private GameObject _hideItemObj = null;

        [Header("Game Events")]
        [SerializeField]
        private GameEventTwoStringInt _getItemCallback = null;

        #endregion

        #region InteractableObject

        public override InteractionType InteractType => InteractionType.PickUpItem;

        public override void OnProcess(string entityID)
        {
#if UNITY_EDITOR
            Debug.Log($"[DEBUG] Collected: {_itemName} - {_amount}");
#endif
            if (_hideOnCollected && _hideItemObj != null)
                _hideItemObj.SetActive(false);
            else if (_hideOnCollected && _hideItemObj == null)
                gameObject.SetActive(false);

            _getItemCallback.Invoke(entityID, _itemName, _amount);
            Finish();
        }

        #endregion
    }
}
