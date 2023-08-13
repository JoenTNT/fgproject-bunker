using JT.GameEvents;
using UnityEngine;

namespace JT.FGP.Dialogue
{
    /// <summary>
    /// An adapter to connect between entity character and dialogue system.
    /// </summary>
    public class InteractionDialogueAdapter : MonoBehaviour
    {
        #region Variable

        [Header("Properties")]
        [SerializeField]
        private bool _lockControlWhenDialogue = true;

        [Header("Game Events")]
        [SerializeField]
        private GameEventString _onChooseRunningDialogue = null;

        [SerializeField]
        private GameEventThreeString _onStartInteractDialogue = null;

        [SerializeField]
        private GameEventNoParam _onDialogueProcessEnded = null;

        [SerializeField]
        private GameEventBool _lockControllerCallback = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events
            if (_onStartInteractDialogue != null)
                _onStartInteractDialogue.AddListener(ListenOnStartDialogue, this);
            if (_onDialogueProcessEnded != null)
                _onDialogueProcessEnded.AddListener(ListenOnDialogueProcessEnded, this);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            if (_onStartInteractDialogue != null)
                _onStartInteractDialogue.RemoveListener(ListenOnStartDialogue, this);
            if (_onDialogueProcessEnded != null)
                _onDialogueProcessEnded.RemoveListener(ListenOnDialogueProcessEnded, this);
        }

        #endregion

        #region Main

        private void ListenOnStartDialogue(string entityID, string targetID, string dialogueID)
        {
            if (_onChooseRunningDialogue == null) return;

            _onChooseRunningDialogue.Invoke(dialogueID, this);

            if (!_lockControlWhenDialogue) return;
            if (_lockControllerCallback == null) return;

            _lockControllerCallback.Invoke(true);
        }

        private void ListenOnDialogueProcessEnded()
        {
            if (!_lockControlWhenDialogue) return;
            if (_lockControllerCallback == null) return;

            _lockControllerCallback.Invoke(false);
        }

        #endregion
    }
}
