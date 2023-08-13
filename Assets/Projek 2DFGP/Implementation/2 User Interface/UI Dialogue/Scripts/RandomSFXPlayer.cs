#if FMOD
using FMODUnity;
#endif
using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Play random sound between registered clips.
    /// </summary>
    public class RandomSFXPlayer : MonoBehaviour
    {
        #region Variable

        [Header("Requirements")]
        [SerializeField]
        private EntityID _soundID = null;
#if FMOD
        [SerializeField]
        private StudioEventEmitter _audioSource = null;
#else
        [SerializeField]
        private AudioSource _audioSource = null;

        [SerializeField]
        private AudioClip[] _soundClip = null;
#endif
        [Header("Game Events")]
        [SerializeField]
        private GameEventString _onPlayRandomSound = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events
            _onPlayRandomSound.AddListener(ListenOnPlayRandomSound);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _onPlayRandomSound.RemoveListener(ListenOnPlayRandomSound);
        }

        //private void Start()
        //{
        //    // TEMPORARY: Choose sample sound index 20 times, DON'T PLAY WHEN THE CLIPS ARE FILLED
        //    for (int i = 0; i < 20; i++)
        //        PlayRandom();
        //}
#if !FMOD
        private void Reset()
        {
            if (_audioSource == null)
                TryGetComponent(out _audioSource);
        }
#endif
        #endregion

        #region Main

        private void ListenOnPlayRandomSound(string soundID)
        {
            if (_soundID.ID != soundID) return;

            PlayRandom();
        }

        public void PlayRandom()
        {
#if FMOD
            _audioSource.Play();
#else
            if (_soundClip.Length == 0) return;

            int soundIndex = Random.Range(0, _soundClip.Length);
#if UNITY_EDITOR
            //Debug.Log($"Choosen Sound Index = {soundIndex}");
#endif
            if (_soundClip[soundIndex] == null) return;
#if UNITY_EDITOR
            //Debug.Log($"Play Sound: {_soundClip[soundIndex]}");
#endif
            _audioSource.PlayOneShot(_soundClip[soundIndex]);
#endif
        }

        #endregion
    }
}
