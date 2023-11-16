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

        #endregion

        #region Mono

        private void Start()
        {
            _data.AudioData.LoadData();
            _onAudioLoaded.Invoke();
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

        #endregion

        #region Properties

        public AudioSettingValueData AudioData => _audioData;

        #endregion

        #region Main

        #endregion
    }
}
