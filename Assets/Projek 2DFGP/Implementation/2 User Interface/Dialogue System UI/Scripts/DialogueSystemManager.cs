using JT.GameEvents;
using UnityEngine;

namespace JT.FGP.Dialogue
{
    /// <summary>
    /// Main game dialogue system.
    /// </summary>
    public class DialogueSystemManager : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        private DialogueSystemManagerData _data = null;

        [Header("Game Events")]
        // From Dialogue System to Dialogue interface
        [SerializeField]
        private GameEventUnityObject _runDialogueInterfaceCallback = null;

        // From Outside to Dialogue System
        [SerializeField]
        private GameEventString _onChooseRunningDialogue = null;

        [SerializeField]
        private GameEventNoParam _endOfDialogueSequenceCallback = null;

        [SerializeField]
        private GameEventNoParam _onDialogueProcessEnded = null;

        // Temporary variable data
        //private string _choosenChoice = string.Empty;

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events
            _onChooseRunningDialogue.AddListener(ListenOnChooseRunningDialogue, this);
            _endOfDialogueSequenceCallback.AddListener(ListenEndOfDialogueSequenceCallback, this);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _onChooseRunningDialogue.RemoveListener(ListenOnChooseRunningDialogue, this);
            _endOfDialogueSequenceCallback.RemoveListener(ListenEndOfDialogueSequenceCallback, this);
        }

        private void Start()
        {
            //// TEMPORARY: Test Sample the First Sequence
            //_runDialogueCallback.Invoke(_loadedPack["Conversation 1"]);

            if (_data.DisableOnStart)
                _data.DisableCanvas();
        }

        #endregion

        #region Main

        private void ListenOnChooseRunningDialogue(string dialogueID)
        {
            _data.EnableCanvas();
            _runDialogueInterfaceCallback.Invoke(_data.Pack[dialogueID], this);
        }

        private void ListenEndOfDialogueSequenceCallback()
        {
#if UNITY_EDITOR
            Debug.Log("End of Dialogue!");
#endif
            // TODO: Choosen choices to change sequence
            _data.DisableCanvas();
            _onDialogueProcessEnded.Invoke(this);
        }

        #endregion
    }

    /// <summary>
    /// Handle data for dialogue system manager.
    /// </summary>
    [System.Serializable]
    internal class DialogueSystemManagerData
    {
        #region Variable

        [Header("Requirements")]
        [SerializeField]
        private DialoguePack _loadedPack = null;

        [SerializeField]
        private Canvas _dialogueCanvas = null;

        [Header("Properties")]
        [SerializeField]
        private bool _disableOnStart = true;

        #endregion

        #region Properties

        /// <summary>
        /// Registered dialogue pack that loaded with sequences.
        /// </summary>
        public DialoguePack Pack => _loadedPack;

        /// <summary>
        /// Disable dialogue interface on start.
        /// </summary>
        public bool DisableOnStart => _disableOnStart;

        #endregion

        #region Main

        public void EnableCanvas() => _dialogueCanvas.gameObject.SetActive(true);

        public void DisableCanvas() => _dialogueCanvas.gameObject.SetActive(false);

        #endregion
    }
}
