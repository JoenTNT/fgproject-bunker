using System.Collections.Generic;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Physical ammunition plays audio when hit something.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Audio Clips Collection",
        menuName = "FGP/Miscellaneous/Audio Clips Collection")]
    public sealed class AudioClipCollectionSO : ScriptableObject, IRequiredInitialize
    {
        #region struct

        /// <summary>
        /// An audio key with it's sound.
        /// </summary>
        [System.Serializable]
        private struct KeyAudioPair
        {
            public string key;
            public AudioClip clip;
        }

        #endregion

        #region Variables

        [SerializeField]
        private KeyAudioPair[] _audioKeys = new KeyAudioPair[0];

        // Runtime variable data.
        private Dictionary<string, AudioClip> _clips = null;
        private bool _isInit = false;

        #endregion

        #region IRequiredInitialize

        public bool IsInitialized => _isInit;

        public void Initialize()
        {
            // Ignore if it's already initialized.
            if (_isInit) return;

            // Initialize dictionary.
            _clips = new Dictionary<string, AudioClip>();
            foreach (var ak in _audioKeys)
                _clips[ak.key] = ak.clip;

            // Set status initialized.
            _isInit = true;
        }

        #endregion

        #region Main

        /// <summary>
        /// Set clip at target audio source.
        /// </summary>
        /// <param name="source">Output source</param>
        /// <param name="audioKey">Use registered audio key</param>
        public void SetClip(AudioSource source, string audioKey)
        {
            // Check if audio has been initialized.
            if (!_isInit) Initialize();

            // Assign clip to source.
            source.clip = _clips[audioKey];
        }

        /// <summary>
        /// Play sound after assigning immediately.
        /// </summary>
        /// <param name="source">Output source</param>
        /// <param name="audioKey">Use registered audio key</param>
        public void PlaySound(AudioSource source, string audioKey)
        {
            // Set clip first.
            SetClip(source, audioKey);

            // Then play audio.
            source.Play();
        }

        #endregion
    }
}
