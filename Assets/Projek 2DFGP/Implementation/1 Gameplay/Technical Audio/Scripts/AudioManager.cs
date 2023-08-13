#if FMOD
using FMOD.Studio;
using FMODUnity;
#endif
using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    public class AudioManager : MonoBehaviour
    {
        #region Variable

        [SerializeField]
        private AudioManagerData _data = new AudioManagerData();

        [Header("Game Events")]
        [SerializeField]
        private GameEventNoParam _onAudioLoaded = null;

        // Temporary variable data
#if FMOD
        private Bus _masterBus;
        private Bus _bgmGroupBus;
        private Bus _sfxGroupBus;
#endif
        #endregion

        #region Mono

        private void Awake()
        {
#if FMOD
            _masterBus = RuntimeManager.GetBus("bus:/Master");
            _bgmGroupBus = RuntimeManager.GetBus("bus:/Master/BGM");
            _sfxGroupBus = RuntimeManager.GetBus("bus:/Master/SFX");
#endif
        }

        private void Start()
        {
            _data.AudioData.LoadData();
            _onAudioLoaded.Invoke();
#if FMOD
            if (!_data.BGMEmitter.IsPlaying())
                _data.BGMEmitter.Play();
#endif
        }

        private void Update()
        {
#if FMOD
            _masterBus.setVolume(_data.AudioData.masterVolume);
            _bgmGroupBus.setVolume(_data.AudioData.bgmVolume);
            _sfxGroupBus.setVolume(_data.AudioData.sfxVolume);
#endif
        }

        #endregion
    }

    /// <summary>
    /// Handle data for audio manager.
    /// </summary>
    [System.Serializable]
    internal class AudioManagerData
    {
        #region Variable

        [SerializeField]
        private AudioSettingValueData _audioData = null;
#if FMOD
        [Header("BGM References")]
        [SerializeField]
        private StudioEventEmitter _bgmEmitter = null;

        [SerializeField]
        private string _bgmParameter = string.Empty;

        [SerializeField]
        private float _bgmLabelValue = 0f;
#endif
        #endregion

        #region Properties

        public AudioSettingValueData AudioData => _audioData;
#if FMOD
        public StudioEventEmitter BGMEmitter => _bgmEmitter;

        public string BGMParameter => _bgmParameter;

        public float BGMLabel
        {
            get => _bgmLabelValue;
            set => _bgmLabelValue = value;
        }
#endif
        #endregion

        #region Main

        public void RestartBGM()
        {
#if FMOD
            _bgmEmitter.Stop();
            _bgmEmitter.SetParameter(_bgmParameter, _bgmLabelValue);
            _bgmEmitter.Play();
#else
            
#endif
        }

        #endregion
    }
}
