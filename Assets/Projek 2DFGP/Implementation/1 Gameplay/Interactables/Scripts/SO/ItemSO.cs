using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// An abstract item behaviour.
    /// </summary>
    public abstract class ItemSO : ScriptableObject
    {
        #region Variables

        [Header("Requirements")]
        [Tooltip("Item icon, used both in UI and gameplay world")]
        [SerializeField]
        private Sprite _icon = null;

        [Header("Properties")]
        [SerializeField]
        private string _name = string.Empty;

        [SerializeField, TextArea]
        private string _description = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Item name.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Item description
        /// </summary>
        public string Description => _description;

        /// <summary>
        /// Item icon, used both in UI and gameplay world.
        /// </summary>
        public Sprite Icon => _icon;

        #endregion
    }
}
