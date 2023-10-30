using System.Collections.Generic;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Contains interaction icons.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Interaction Icons",
        menuName = "FGP/UI/Interaction Icons")]
    public sealed class InteractionIconsSO : ScriptableObject
    {
        #region structs

        /// <summary>
        /// Each pair which type of interaction and icon.
        /// </summary>
        [System.Serializable]
        private struct Pair
        {
            #region Variables

            [SerializeField]
            private InteractionType _type;

            [SerializeField]
            private Sprite _icon;

            #endregion

            #region Properties

            /// <summary>
            /// Unique interaction type.
            /// </summary>
            public InteractionType InteractionType => _type;

            /// <summary>
            /// Icon for this type of interaction.
            /// </summary>
            public Sprite Icon => _icon;

            #endregion
        }

        #endregion

        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private Pair[] _interactionIcons = new Pair[0];

        // Runtime variable data.
        private Dictionary<InteractionType, Sprite> _pairs = null;

        #endregion

        #region Properties

        /// <summary>
        /// How many registered icons.
        /// </summary>
        public int Count => _interactionIcons.Length;

        #endregion

        #region Main

        private void Init()
        {
            // Initialize dictionary for better search.
            _pairs = new Dictionary<InteractionType, Sprite>();
            foreach (var p in _interactionIcons)
                _pairs[p.InteractionType] = p.Icon;
        }

        public Sprite GetIcon(InteractionType type)
        {
            if (_pairs == null) Init();
            return _pairs[type];
        }

        #endregion
    }
}
