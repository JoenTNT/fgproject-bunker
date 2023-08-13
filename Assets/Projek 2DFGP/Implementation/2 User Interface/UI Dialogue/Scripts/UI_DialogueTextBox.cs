using JT.GameEvents;
using UnityEngine;

namespace JT.FGP.Dialogue
{
    [RequireComponent(typeof(RectTransform))]
    public class UI_DialogueTextBox : MonoBehaviour
    {
        #region Variable

        [SerializeField]
        private UI_DialogueTextBoxData _data = null;

        [Header("Game Events")]
        [SerializeField]
        private GameEventUnityObject _runDialogueInterfaceCallback = null;

        [SerializeField]
        private GameEventNoParam _onSkipTextAnimation = null;

        [SerializeField]
        private GameEventString _onPlayRandomSound = null;

        //[SerializeField]
        //private GameEventString _onPlayDubbingSound = null;

        [SerializeField]
        private GameEventNoParam _endOfDialogueSequenceCallback = null;

        // Temporary variable data
        private int _currentDialogueIndex = 0;
        private int _countBeforeEmit = 0;

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events
            _runDialogueInterfaceCallback.AddListener(ListenRunDialogueInterfaceCallback);
            _onSkipTextAnimation.AddListener(ListenOnSkipTextAnimation);
            _data.TextAnimation.OnFinish += ListenTextAnimation_OnFinish;
            _data.TextAnimation.OnLetterReveal += ListenTextAnimation_OnLetterReveal;
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _runDialogueInterfaceCallback.RemoveListener(ListenRunDialogueInterfaceCallback);
            _onSkipTextAnimation.RemoveListener(ListenOnSkipTextAnimation);
            _data.TextAnimation.OnFinish -= ListenTextAnimation_OnFinish;
            _data.TextAnimation.OnLetterReveal -= ListenTextAnimation_OnLetterReveal;
        }

        //private void Start()
        //{
        //    // TEMPORARY: Run Sample Content Animation
        //    _countBeforeEmit = 0;
        //    _data.TextAnimation.StartRun();
        //}

        #endregion

        #region Main

        private void ListenRunDialogueInterfaceCallback(Object obj)
        {
            if (obj is not DialogueSequence) return;

            _currentDialogueIndex = 0;
            _data.Sequence = (DialogueSequence)obj;
            _data.TextAnimation.SetText(_data.Sequence.GetContent(_currentDialogueIndex).Text);
            _data.TextAnimation.StartRun();
        }

        private void ListenOnSkipTextAnimation()
        {
            bool skipped = _data.TextAnimation.SkipText();
#if UNITY_EDITOR
            //Debug.Log($"Skipping The Text Animation ({skipped})");
#endif
            if (skipped) return;

            _currentDialogueIndex++;

            if (_currentDialogueIndex >= _data.Sequence.SentenceCount)
            {
                _endOfDialogueSequenceCallback.Invoke();
                // TODO: Disable the Dialogue
                return;
            }

            _countBeforeEmit = 0;
            _data.TextAnimation.SetText(_data.Sequence.GetContent(_currentDialogueIndex).Text);
            _data.TextAnimation.StartRun();
        }

        private void ListenTextAnimation_OnFinish()
        {
#if UNITY_EDITOR
            //Debug.Log($"Text Animation Has Finished Revealing the Content.");
#endif
            _countBeforeEmit = 0;
        }

        private void ListenTextAnimation_OnLetterReveal(char revealChar)
        {
            if (_data.NonLetterDigitIsSilent && !char.IsLetterOrDigit(revealChar)) return;

            _countBeforeEmit++;

            if (_countBeforeEmit < _data.CharPerEmitSound) return;

            _countBeforeEmit = 0;
            ISingleText content = _data.Sequence.GetContent(_currentDialogueIndex);

            if (content is not WrappedSentence) return;

            WrappedSentence sentenceContent = (WrappedSentence)content;

            if (sentenceContent.SoundType != DialogueTextSoundType.LetterVoice) return;

            _onPlayRandomSound.Invoke(sentenceContent.VoiceID);
        }

        #endregion
    }

    /// <summary>
    /// Handle data for UI dialogue text box.
    /// </summary>
    [System.Serializable]
    internal class UI_DialogueTextBoxData
    {
        #region Variable

        [Header("Requirements")]
        [SerializeField]
        private UI_TextAnimation _textAnimation = null;

        [Header("Optional")]
        [SerializeField]
        private DialogueSequence _sequence = null;

        [Header("Sound System")]
        // NOTE: Due to fast sound emit that disturb the ear, this must be implemented.
        [SerializeField]
        private int _charPerEmitSound = 1;

        [SerializeField]
        private bool _nonLetterDigitIsSilent = false;

        #endregion

        #region Properties

        /// <summary>
        /// Dialogue text animation.
        /// </summary>
        public UI_TextAnimation TextAnimation => _textAnimation;

        /// <summary>
        /// Current dialogue sequence.
        /// </summary>
        public DialogueSequence Sequence
        {
            get => _sequence;
            set => _sequence = value;
        }

        /// <summary>
        /// To count character reveal before emitting sound.
        /// </summary>
        public int CharPerEmitSound => _charPerEmitSound;

        /// <summary>
        /// When non letter or digit character revealed, then don't emit the sound.
        /// </summary>
        public bool NonLetterDigitIsSilent => _nonLetterDigitIsSilent;

        #endregion
    }
}
