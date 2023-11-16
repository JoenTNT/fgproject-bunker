using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Ranged weapon shooting sfx audio handler.
    /// </summary>
    [System.Serializable]
    public struct RW_Audio
    {
        #region Variables

        [SerializeField]
        private AudioClip _preShotClip;

        [SerializeField]
        private AudioClip _onShotClip;

        [SerializeField]
        private AudioClip _afterShotClip;

        #endregion

        #region Main

        public void PlayBeforeShotClip(AudioSource source)
        {
            if (_preShotClip == null) return;
            source?.PlayOneShot(_preShotClip);
        }

        public void PlayOnShotClip(AudioSource source)
        {
            if (_onShotClip == null) return;
            source?.PlayOneShot(_onShotClip);
        }

        public void PlayAfterShotClip(AudioSource source)
        {
            if (_afterShotClip == null) return;
            source?.PlayOneShot(_afterShotClip);
        }

        #endregion
    }
}
