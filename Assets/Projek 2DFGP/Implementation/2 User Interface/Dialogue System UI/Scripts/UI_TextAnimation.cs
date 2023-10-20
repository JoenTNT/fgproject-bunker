using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace JT.FGP.Dialogue
{
#if UNITY_2021_1_OR_NEWER
    [RequireComponent(typeof(TextMeshProUGUI))]
#elif !UNITY_2018_3_OR_NEWER
    [RequireComponent(typeof(UnityEngine.UI.Text))]
#endif
    public class UI_TextAnimation : MonoBehaviour, IProcessHandler, IRunnableCommand, IWaitUntilFinishHandler
    {
        #region Variable

        /// <summary>
        /// Event called when character revealed on the text while animation is running.
        /// </summary>
        public event Action<char> OnLetterReveal;

        ///// <summary>
        ///// Event called when markup executed.
        ///// </summary>
        //public event Action<string> OnMarkupCommandExecute;
#if UNITY_2021_1_OR_NEWER
        [SerializeField]
        private TextMeshProUGUI _uiText = null;
#elif !UNITY_2018_3_OR_NEWER
        [SerializeField]
        private UnityEngine.UI.Text _uiText = null;
#endif
        [Tooltip("Animate the Text Content, Only for Debugging")]
        [SerializeField, TextArea(3, 10)]
        private string _textContent = string.Empty;

        /// <summary>
        /// Text Speed = Letters / Second.
        /// </summary>
        [SerializeField]
        private int _textSpeed = 15;

        [SerializeField]
        private bool _isInterrupable = false;

        // Temporary variable data
        private IEnumerator _textAnimationRoutine = null;
        private float _newLetterInSecond = 0f;
        private int _contentIndex = 0;
        private bool _isSkip = false;
        private bool _isRunning = false;
        private bool _isFinish = false;

        #endregion

        #region IProcessHandler

        public bool IsProcessRunning => _isRunning;

        public bool ProcessInterruptable
        {
            get => _isInterrupable;
            set => _isInterrupable = value;
        }

        public void OnStartProcess()
        {
            _contentIndex = -1;
            _uiText.text = string.Empty;
            _newLetterInSecond = 1f / _textSpeed;
        }

        public void OnProcess()
        {
            _newLetterInSecond -= Time.deltaTime;

            if (_newLetterInSecond > 0f) return;

            _contentIndex++;
            _uiText.text += _textContent[_contentIndex];
            OnLetterReveal?.Invoke(_textContent[_contentIndex]);
            _newLetterInSecond = 1f / _textSpeed;

            if (_contentIndex < _textContent.Length - 1) return;

            _isFinish = true;
            OnFinish?.Invoke();
        }

        public void OnEndProcess()
        {
            _isRunning = false;
            _textAnimationRoutine = null;
        }

        #endregion

        #region IRunnableCommand

        /// <summary>
        /// Start run the dialogue animation.
        /// This will not work if the current text animation is running.
        /// </summary>
        public void StartRun()
        {
            if (_textAnimationRoutine != null) return;

            _isRunning = true;
            _textAnimationRoutine = TextAnimationRoutine();
            StartCoroutine(_textAnimationRoutine);
        }

        /// <summary>
        /// Stop the current dialogue animation.
        /// This will only work if the dialogue text animation is interrupable.
        /// </summary>
        public void StopRun()
        {
            if (_textAnimationRoutine == null) return;
            if (!_isInterrupable) return;

            _isRunning = false;
            StopCoroutine(_textAnimationRoutine);
            _textAnimationRoutine = null;
        }

        #endregion

        #region IWaitUntilFinishHandler

        public event Action OnFinish;
        
        public bool IsFinished => _isFinish;

        #endregion

        #region Main

        /// <summary>
        /// Set text animation which will be revealed/animated.
        /// Make sure you check if the current text animation is currently running to prevent unwanted error.
        /// </summary>
        /// <param name="text">String text content</param>
        /// <param name="reset">Reset to empty text, default is true</param>
        public void SetText(string text, bool reset = true)
        {
            _textContent = text;

            if (reset) _isFinish = false;
        }

        /// <summary>
        /// Skip the animation and reveal all the text immediately.
        /// </summary>
        /// <returns>Text animation successfully skipped</returns>
        public bool SkipText() => _isSkip = _isRunning;

        private IEnumerator TextAnimationRoutine()
        {
            OnStartProcess();

            while (!_isFinish)
            {
                if (_isSkip)
                {
                    _uiText.text = _textContent;
                    _isFinish = true;
                    break;
                }

                OnProcess();
                yield return null;
            }

            OnEndProcess();
        }

        #endregion
    }
}
