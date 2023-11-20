using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Manages audio informations.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        #region Variable

        [Header("Requirements")]
        [SerializeField]
        private AudioSource _bgmSource = null;

        [SerializeField]
        private AudioClipCollectionSO _bgmClips = null;

        [SerializeField]
        private RuntimeAudioSettingSO _audioData = null;

        [Header("Properties")]
        [SerializeField]
        private string _loopAudioKey = null;
        
        [Header("Game Events")]
        [SerializeField]
        private GameEventNoParam _onAudioLoaded = null;

        #endregion

        #region Properties

        /// <summary>
        /// Registered BGM clips collection.
        /// </summary>
        public AudioClipCollectionSO BGMClips => _bgmClips;

        /// <summary>
        /// Runtime audio value informations.
        /// </summary>
        public RuntimeAudioSettingSO AudioData => _audioData;

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events.
            _audioData.OnDataUpdated += ListenOnDataUpdated;
        }

        private void OnDestroy()
        {
            // Unsubscribe events.
            _audioData.OnDataUpdated -= ListenOnDataUpdated;
        }

        private void Start()
        {
            // Load sound setting data.
            _audioData.LoadData();

            // Call for loaded audio values and data.
            _onAudioLoaded.Invoke();

            // Set BGM audio.
            _bgmSource.loop = true;
            _bgmClips.PlaySound(_bgmSource, _loopAudioKey);
        }

        #endregion

        #region Main

        private void ListenOnDataUpdated()
        {
            // Always assign BGM volume.
            _bgmSource.volume = _audioData.MasterVolume * _audioData.BGMVolume;
        }

        /// <summary>
        /// To change background music.
        /// </summary>
        /// <param name="key">Registered specific key</param>
        public void ChangeBGM(string key)
        {
            _bgmSource.Stop();
            _bgmSource.time = 0;
            _bgmClips.PlaySound(_bgmSource, key);
        }

        /// <summary>
        /// To change background music.
        /// </summary>
        /// <param name="index">Registered index</param>
        public void ChangeBGM(int index)
        {
            _bgmSource.Stop();
            _bgmSource.time = 0;
            _bgmClips.PlaySound(_bgmSource, index);
        }

        #endregion
    }
}
