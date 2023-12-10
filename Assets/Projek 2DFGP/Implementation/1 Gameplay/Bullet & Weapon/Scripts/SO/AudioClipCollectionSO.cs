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
    public sealed class AudioClipCollectionSO : ScriptableObject
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

        #endregion

        #region Properties

        /// <summary>
        /// Use this properties to initialize dictionary.
        /// </summary>
        private Dictionary<string, AudioClip> DC
        {
            get
            {
                if (_clips != null) return _clips;
                _clips = new Dictionary<string, AudioClip>();
                foreach (var ak in _audioKeys)
                    _clips[ak.key] = ak.clip;
                return _clips;
            }
        }

        /// <summary>
        /// Registered clips count.
        /// </summary>
        public int ClipCount => _audioKeys.Length;

        #endregion

        #region Main

        /// <summary>
        /// Get clip by key.
        /// </summary>
        public AudioClip GetClip(string key)
        {
            // Return the audio clip.
            return DC[key];
        }

        /// <summary>
        /// Get clip by index.
        /// </summary>
        public AudioClip GetClip(int index) => _audioKeys[index].clip;

        /// <summary>
        /// Set clip at target audio source.
        /// </summary>
        /// <param name="source">Output source</param>
        /// <param name="audioKey">Use registered audio key</param>
        public void SetClip(AudioSource source, string audioKey)
        {
            // Assign clip to source.
            source.clip = DC[audioKey];
        }

        /// <summary>
        /// Set clip at target audio source.
        /// </summary>
        /// <param name="source">Output source</param>
        /// <param name="audioIndex">Use registered audio index</param>
        public void SetClip(AudioSource source, int audioIndex)
        {
            // Assign clip to source.
            source.clip = _audioKeys[audioIndex].clip;
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

        /// <summary>
        /// Play sound after assigning immediately.
        /// </summary>
        /// <param name="source">Output source</param>
        /// <param name="audioIndex">Use registered audio index</param>
        public void PlaySound(AudioSource source, int audioIndex)
        {
            // Set clip first.
            SetClip(source, audioIndex);

            // Then play audio.
            source.Play();
        }

        #endregion
    }
}
