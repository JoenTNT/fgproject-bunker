using UnityEngine;

namespace JT.FGP.Dialogue
{
    /// <summary>
    /// Dialogue type sound.
    /// </summary>
    public enum DialogueTextSoundType
    {
        LetterVoice, // Each character emits sound
        DubbingVoice, // Runs on start, let the dubbing sound running until the end
    }

    /// <summary>
    /// Dialogue sentence type.
    /// </summary>
    [System.Serializable]
    public struct WrappedSentence : ISingleText
    {
        #region Variable

        [Header("Misc")]
        [SerializeField]
        private Sprite _characterProfile;

        [SerializeField]
        private string _characterName;

        [Header("Contents")]
        [SerializeField, TextArea(3, 10)]
        private string _text;

        [Header("Sound Attributes")]
        [SerializeField]
        private DialogueTextSoundType _soundType;

        [SerializeField]
        private string _voiceID;

        #endregion

        #region Properties

        public Sprite CharacterProfile => _characterProfile;

        public string CharacterName => _characterName;

        public DialogueTextSoundType SoundType => _soundType;

        public string VoiceID => _voiceID;

        #endregion

        #region ISingleText

        public string Text => _text;

        #endregion
    }
}
