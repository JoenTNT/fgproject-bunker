using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// For physical entity to open dialogue interaction.
    /// </summary>
    public class OpenDialogue : InteractableObject
    {
        #region Variable

        [Header("Requirements")]
        [SerializeField]
        private EntityID _entityID = null;

        [Header("Properties")]
        [SerializeField]
        private string _dialogueID = string.Empty;

        [Header("Game Events")]
        [SerializeField]
        private GameEventTwoString _changeDialogueIDCallback = null;

        /// <summary>
        /// Event called to start a dialogue conversation between 2 entity.
        /// </summary>
        /// <example>
        /// _onStartDialogue(Player, Target, DialogueID)
        /// </example>
        [SerializeField]
        private GameEventThreeString _onStartInteractDialogue = null;

        /// <summary>
        /// Event called to send notice to both entity that the dialogue has ended.
        /// </summary>
        /// <example>
        /// _onEndDialogue(Player, Target, DialogueID)
        /// </example>
        [SerializeField]
        private GameEventThreeString _onEndInteractDialogue = null;

        [SerializeField]
        private GameEventNoParam _onDialogueProcessEnded = null;

        #endregion

        #region Properties

        /// <summary>
        /// Set this property before running dialogue to change conversation.
        /// </summary>
        public string DialogueID
        {
            get => _dialogueID;
            set => _dialogueID = value;
        }

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events
            _changeDialogueIDCallback.AddListener(ListenChangeDialogueIDCallback);
            _onDialogueProcessEnded.AddListener(ListenOnDialogueProcessEnded);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _changeDialogueIDCallback.RemoveListener(ListenChangeDialogueIDCallback);
            _onDialogueProcessEnded.RemoveListener(ListenOnDialogueProcessEnded);
        }

        #endregion

        #region InteractableObject

        public override InteractionType InteractType => InteractionType.Talk;

        /// <param name="entityID">Entity who interact with this entity, not this entity</param>
        public override void OnStartProcess(string entityID)
        {
#if UNITY_EDITOR
            Debug.Log($"{this} Start Interact with: {entityID} (Dialogue: {_dialogueID})");
#endif
            _onStartInteractDialogue.Invoke(entityID, _entityID.ID, _dialogueID);
        }

        /// <param name="entityID">Entity who interact with this entity, not this entity</param>
        public override void OnEndProcess(string entityID)
        {
#if UNITY_EDITOR
            Debug.Log($"{this} End Interact with: {entityID} (Dialogue: {_dialogueID})");
#endif
            _onEndInteractDialogue.Invoke(entityID, _entityID.ID, _dialogueID);
        }

        #endregion

        #region Main

        /// <param name="entityID">This entity</param>
        private void ListenChangeDialogueIDCallback(string entityID, string dialogueID)
        {
            if (_entityID.ID != entityID) return;

            _dialogueID = dialogueID;
        }

        private void ListenOnDialogueProcessEnded() => Finish();

        #endregion
    }
}
