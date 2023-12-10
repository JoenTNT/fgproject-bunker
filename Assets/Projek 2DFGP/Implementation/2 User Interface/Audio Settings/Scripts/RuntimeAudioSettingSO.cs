using UnityEngine;

namespace JT.FGP
{
    [CreateAssetMenu(
        fileName = "Runtime Audio Settings",
        menuName = "FGP/Settings/Runtime Audio Settings")]
    public class RuntimeAudioSettingSO : ScriptableObject, ISaveDataHandler, ILoadDataHandler
    {
        #region Variable

        private const string MASTER_KEY = "MASTER";
        private const string BGM_KEY = "BGM";
        private const string SFX_KEY = "SFX";

        public event System.Action OnDataUpdated;

        [Header("Initial Properties")]
        [SerializeField, Min(0f)]
        private float _masterVolume = 1f;

        [SerializeField, Min(0f)]
        private float _bgmVolume = 1f;

        [SerializeField, Min(0f)]
        private float _sfxVolume = 1f;

        #endregion

        #region Properties

        /// <summary>
        /// Volume multiplier for all volumes.
        /// </summary>
        public float MasterVolume
        {
            get => _masterVolume;
            set => _masterVolume = value < 0f ? 0f : value > 1f ? 1f : value;
        }

        /// <summary>
        /// Background music volume.
        /// </summary>
        public float BGMVolume
        {
            get => _bgmVolume;
            set => _bgmVolume = value < 0f ? 0f : value > 1f ? 1f : value;
        }

        /// <summary>
        /// Sound effects volume.
        /// </summary>
        public float SFXVolume
        {
            get => _sfxVolume;
            set => _sfxVolume = value < 0f ? 0f : value > 1f ? 1f : value;
        }

        #endregion

        #region ISaveDataHandler

        public void SaveData()
        {
            PlayerPrefs.SetFloat(MASTER_KEY, _masterVolume);
            PlayerPrefs.SetFloat(BGM_KEY, _bgmVolume);
            PlayerPrefs.SetFloat(SFX_KEY, _sfxVolume);
            PlayerPrefs.Save();

            OnDataUpdated?.Invoke();
        }

        #endregion

        #region ILoadDataHandler

        public void LoadData()
        {
            _masterVolume = PlayerPrefs.GetFloat(MASTER_KEY);
            _bgmVolume = PlayerPrefs.GetFloat(BGM_KEY);
            _sfxVolume = PlayerPrefs.GetFloat(SFX_KEY);

            OnDataUpdated?.Invoke();
        }

        #endregion
#if UNITY_EDITOR
        #region SO

        private void OnValidate()
        {
            // Set max volume.
            if (_masterVolume > 1f) _masterVolume = 1f;
            if (_bgmVolume > 1f) _bgmVolume = 1f;
            if (_sfxVolume > 1f) _sfxVolume = 1f;
        }

        #endregion
#endif
    }
}
