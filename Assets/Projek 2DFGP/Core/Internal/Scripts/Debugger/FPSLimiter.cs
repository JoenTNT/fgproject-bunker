using UnityEngine;

namespace JT.Debugger
{
    /// <summary>
    /// This component limits the FPS of the game.
    /// </summary>
    public sealed class FPSLimiter : MonoBehaviour
    {
        #region Variables

        [Header("Properties")]
        [SerializeField]
        private int _fpsLimit = 60;

        [SerializeField]
        private bool _isActive = true;

        #endregion

        #region Mono

        private void Update()
        {
            if (_isActive)
                Application.targetFrameRate = _fpsLimit;
        }

        #endregion
    }
}

