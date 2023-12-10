using JT.GameEvents;
using UnityEngine;
using UnityEngine.UI;

namespace JT.FGP
{
    [RequireComponent(typeof(RectTransform))]
    public class UI_VolumeSetting : MonoBehaviour
    {
        #region enum

        /// <summary>
        /// Existing audio type in game.
        /// </summary>
        public enum AudioType { Master, BGM, SFX, }

        #endregion

        #region Variable

        [Header("Properties")]
        [SerializeField]
        private RuntimeAudioSettingSO _settingData = null;

        [SerializeField]
        private Slider _volumeSlider = null;

        [SerializeField]
        private AudioType _audioType = AudioType.Master;

        [Header("Game Events")]
        [SerializeField]
        private GameEventNoParam _onAudioLoaded = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events
            _onAudioLoaded.AddListener(ListenOnAudioLoaded);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _onAudioLoaded.RemoveListener(ListenOnAudioLoaded);
        }

        private void Start() => SyncSlider();

        #endregion

        #region Main

        private void ListenOnAudioLoaded() => SyncSlider();

        private void SyncSlider()
        {
            switch (_audioType)
            {
                case AudioType.Master:
                    _volumeSlider.value = _settingData.MasterVolume;
                    break;

                case AudioType.BGM:
                    _volumeSlider.value = _settingData.BGMVolume;
                    break;

                case AudioType.SFX:
                    _volumeSlider.value = _settingData.SFXVolume;
                    break;
            }
        }

        public void SetVolume(float volume)
        {
            switch (_audioType)
            {
                case AudioType.Master:
                    _settingData.MasterVolume = volume;
                    break;

                case AudioType.BGM:
                    _settingData.BGMVolume = volume;
                    break;

                case AudioType.SFX:
                    _settingData.SFXVolume = volume;
                    break;
            }
        }

        #endregion
    }
}
