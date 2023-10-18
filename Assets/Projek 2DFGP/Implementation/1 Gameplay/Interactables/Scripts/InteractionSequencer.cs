using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle multiple interaction in sequence.
    /// </summary>
    public sealed class InteractionSequencer : InteractableComponent
    {
        #region structs

        /// <summary>
        /// Interaction sequence template.
        /// </summary>
        [System.Serializable]
        private struct SequenceTemplate
        {
            #region Variables

            [Tooltip("All must be filled and no single null object exists.")]
            [SerializeField]
            private InteractableComponent[] _interactions;

            #endregion

            #region Properties

            /// <summary>
            /// How many interactions in one sequence.
            /// All must be filled.
            /// </summary>
            public int InteractionLength => _interactions.Length;

            #endregion

            #region Main

            /// <summary>
            /// Get single component.
            /// </summary>
            /// <param name="onSequenceIndex"></param>
            /// <exception cref="System.IndexOutOfRangeException">
            /// May occur index out of range, use interaction length
            /// </exception>
            /// <returns>Interaction component reference.</returns>
            public InteractableComponent GetInteractableComponent(int onSequenceIndex)
                => _interactions[onSequenceIndex];

            #endregion
        }

        #endregion

        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private EntityID _ownerID = null;

        [SerializeField]
        private SequenceTemplate[] _sequences = new SequenceTemplate[0];

        [Header("Properties")]
        [SerializeField, Min(-1)]
        private int _startingSequenceIndex = 0;

        [Header("Game Events")]
        [SerializeField]
        private GameEventTwoString _onInteractionSequenceStarted = null;

        [SerializeField]
        private GameEventTwoString _onInteractionSequenceEnded = null;

        // Runtime variable data.
        private SequenceTemplate _usedSequence;
        private int _choosenSequenceIndex = -1;
        private int _currentInteractionIndex = 0;

        #endregion

        #region Mono

        private void Awake()
        {
            // Use first sequence
            _choosenSequenceIndex = _startingSequenceIndex;
            _usedSequence = _sequences[_choosenSequenceIndex];
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_startingSequenceIndex >= _sequences.Length)
                _startingSequenceIndex = _sequences.Length - 1;
        }
#endif
        #endregion

        #region InteractableComponent

        public override InteractionType Type
            => _usedSequence.GetInteractableComponent(_currentInteractionIndex).Type;

        public override bool Interact(string entity)
        {
            return true;
        }

        #endregion
    }
}
