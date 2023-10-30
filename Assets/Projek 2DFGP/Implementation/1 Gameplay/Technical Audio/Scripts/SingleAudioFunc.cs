#if FMOD
using FMODUnity;
#endif
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle running single sound audio.
    /// </summary>
#if FMOD
    [RequireComponent(typeof(StudioEventEmitter))]
#else
    [RequireComponent(typeof(AudioSource))]
#endif
    public sealed class SingleAudioFunc : MonoBehaviour
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private StudioEventEmitter _emitter = null;

        #endregion

        #region Main

        public void RunAudio()
        {

        }

        #endregion
    }
}
