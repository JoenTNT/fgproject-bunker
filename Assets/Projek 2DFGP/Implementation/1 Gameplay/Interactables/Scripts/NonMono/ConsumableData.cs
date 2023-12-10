using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Data structure of consumable.
    /// </summary>
    [System.Serializable]
    public struct ConsumableData
    {
        #region Variables

        [SerializeField]
        private ConsumableItemType _type;

        [SerializeField]
        private float _gainValue;

        #endregion

        #region Properties

        /// <summary>
        /// Consumer get this item type.
        /// </summary>
        public ConsumableItemType Type => _type;

        /// <summary>
        /// How much consumer gain value of the selected type.
        /// </summary>
        public float GainValue => _gainValue;

        #endregion
    }
}