using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// A consumable item that contains value and one time use only.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Consumable Item",
        menuName = "FGP/Item/Consumable Item")]
    public sealed class ConsumableItemSO : ItemSO, IConsumable<ConsumableData[]>
    {
        #region Variable

        [Header("Consumable Properties")]
        [SerializeField]
        private ConsumableData[] _consumables = new ConsumableData[0];

        [SerializeField]
        private bool _isOneTimeOnly = true;

        #endregion

        #region IConsumable

        public ConsumableData[] ConsumerGain => _consumables;

        public bool IsOneTimeOnly => _isOneTimeOnly;

        #endregion
    }
}
