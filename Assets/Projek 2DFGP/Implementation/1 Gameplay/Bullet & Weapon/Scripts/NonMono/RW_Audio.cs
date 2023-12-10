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
        private AudioClipCollectionSO _preShotClip;

        [SerializeField]
        private AudioClipCollectionSO _onShotClip;

        [SerializeField]
        private AudioClipCollectionSO _afterShotClip;

        [SerializeField, Min(-1f)]
        private float _callPreAfterOn;

        #endregion

        #region Properties

        /// <summary>
        /// Time run after shot before running the pre sound again.
        /// </summary>
        public float CallPreAfterOn => _callPreAfterOn;

        #endregion

        #region Main

        public void PlayBeforeShotClip(AudioSource source)
        {
            if (_preShotClip == null) return;
            if (_preShotClip.ClipCount == 0) return;

            if (_preShotClip.ClipCount > 1)
                source?.PlayOneShot(_preShotClip.GetClip(Random.Range(0, _preShotClip.ClipCount)));
            else source?.PlayOneShot(_preShotClip.GetClip(0));
        }

        public void PlayOnShotClip(AudioSource source)
        {
            if (_onShotClip == null) return;
            if (_onShotClip.ClipCount == 0) return;

            if (_onShotClip.ClipCount > 1)
                source?.PlayOneShot(_onShotClip.GetClip(Random.Range(0, _onShotClip.ClipCount)));
            else source?.PlayOneShot(_onShotClip.GetClip(0));
        }

        public void PlayAfterShotClip(AudioSource source)
        {
            if (_afterShotClip == null) return;
            if (_afterShotClip.ClipCount == 0) return;

            if (_afterShotClip.ClipCount > 1)
                source?.PlayOneShot(_afterShotClip.GetClip(Random.Range(0, _afterShotClip.ClipCount)));
            else source?.PlayOneShot(_afterShotClip.GetClip(0));
        }

        #endregion
    }
}
