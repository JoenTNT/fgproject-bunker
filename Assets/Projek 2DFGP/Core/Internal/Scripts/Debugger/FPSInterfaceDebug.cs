using UnityEngine;

namespace JT.Debugger
{
    /// <summary>
    /// Shows information in the scene about FPS.
    /// </summary>
    public class FPSInterfaceDebug : MonoBehaviour
    {
        #region Variables

        [Header("Properties")]
        [SerializeField, Min(.01f)]
        private float _recordEverySeconds = 1f;

        // Runtime variable data.
        private GUIStyle _guiRenderStyle = null;
        private float _recordInSecond = 1f;
        private float _fpsResult = 0f;

        #endregion

        #region Mono

        private void Update()
        {
            // Tick unscaled delta time.
            _recordInSecond -= Time.unscaledDeltaTime;

            // Check record when ready.
            if (_recordInSecond <= 0f)
            {
                // Record value.
                _fpsResult = 1f / Time.smoothDeltaTime;

                // Restart countdown.
                _recordInSecond = _recordEverySeconds;
            }
        }

        private void OnGUI()
        {
            // Create GUI style.
            if (_guiRenderStyle == null)
            {
                _guiRenderStyle = new GUIStyle();
                _guiRenderStyle.fontSize = 48;
                _guiRenderStyle.fontStyle = FontStyle.Bold;
            }

            // Render GUI.
            _guiRenderStyle.normal.textColor = _fpsResult < 15 ? Color.red : _fpsResult < 30
                ? Color.yellow : Color.green;
            GUIContent content = new GUIContent($"FPS: {(int)_fpsResult}");
            GUI.Box(new Rect(Screen.width - 276f, 128f, 256, 64), content, _guiRenderStyle);
        }

        #endregion
    }
}