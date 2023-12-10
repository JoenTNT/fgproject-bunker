using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle running single sound audio.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public sealed class SingleAudioFunc : MonoBehaviour
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private AudioSource _source = null;

        #endregion

        #region Mono

        private void Reset() => TryGetComponent(out _source);

        #endregion

        #region Main

        public void PlayAudio(AudioClip clip)
        {
            _source.clip = clip;
            _source.Play();
        }

        public void PlayOneShot(AudioClip clip) => _source.PlayOneShot(clip);

        #endregion
    }
}
