using UnityEngine;

namespace JT.FGP
{
    [CreateAssetMenu(
        fileName = "Audio Setting Value Data",
        menuName = "FGP/Audio Setting Value Data")]
    public class AudioSettingValueData : ScriptableObject, ISaveDataHandler, ILoadDataHandler
    {
        #region Variable

        private const string MASTER_KEY = "MASTER";
        private const string BGM_KEY = "BGM";
        private const string SFX_KEY = "SFX";

        public event System.Action OnDataUpdated;

        public float masterVolume = 1f;
        public float bgmVolume = 1f;
        public float sfxVolume = 1f;

        #endregion

        #region ISaveDataHandler

        public void SaveData()
        {
            PlayerPrefs.SetFloat(MASTER_KEY, masterVolume);
            PlayerPrefs.SetFloat(BGM_KEY, bgmVolume);
            PlayerPrefs.SetFloat(SFX_KEY, sfxVolume);
            PlayerPrefs.Save();

            OnDataUpdated?.Invoke();
        }

        #endregion

        #region ILoadDataHandler

        public void LoadData()
        {
            masterVolume = PlayerPrefs.GetFloat(MASTER_KEY);
            bgmVolume = PlayerPrefs.GetFloat(BGM_KEY);
            sfxVolume = PlayerPrefs.GetFloat(SFX_KEY);

            OnDataUpdated?.Invoke();
        }

        #endregion
    }
}
