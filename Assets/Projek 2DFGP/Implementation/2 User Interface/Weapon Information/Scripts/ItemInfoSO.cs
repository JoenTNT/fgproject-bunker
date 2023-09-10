using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Running on the start of the game to give starter pack for players.
    /// </summary>
    //[CreateAssetMenu(
    //    fileName = "New Item Info",
    //    menuName = "FGP/Item/Item Info")]
    public abstract class ItemInfoSO : ScriptableObject
    {
        #region Variables

        [SerializeField]
        private string _itemKey = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Type of item info.
        /// </summary>
        public abstract ItemType Type { get; }

        #endregion
    }
}
