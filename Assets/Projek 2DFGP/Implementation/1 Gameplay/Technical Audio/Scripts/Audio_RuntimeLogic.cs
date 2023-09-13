#if FMOD
using FMODUnity;
#endif
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace JT.FGP
{
    /// <summary>
    /// Handle runtime sound logic.
    /// </summary>
#if FMOD
    [RequireComponent(typeof(StudioEventEmitter))]
#else
    [RequireComponent(typeof(AudioSource))]
#endif
    public class Audio_RuntimeLogic : MonoBehaviour, IRuntimeHandler, IRuntimeHandler<int>
    {
        #region Variables

        [Header("Requirements")]
#if FMOD
        [SerializeField]
        private StudioEventEmitter _emitter = null;
#else
        [SerializeField]
        private AudioSource _source = null;
#endif
        [Header("Properties")]
        [SerializeField]
        private string _parameterName = string.Empty;

        [SerializeField]
        private int _preDefaultValue = 0, _loopDefaultValue = 0, _postDefaultValue = 0;

        [SerializeField]
        private bool _initOnAwake = true;

        [Header("Game Events")]
        [SerializeField]
        private UnityEvent<int> _onRunPreSound = new();

        [SerializeField]
        private UnityEvent<int> _onRunSoundLoop = new();

        [SerializeField]
        private UnityEvent<int> _onRunPostSound = new();
#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField]
        private bool _debugControl = false;

        [SerializeField]
        private KeyCode _pressKeyToRun = KeyCode.Mouse0;
#endif
        // Runtime variable data.
        private ParamRef _selectedParameter = null;
#if UNITY_EDITOR
        private IEnumerator _debugRuntimeRoutine = null;
#endif
        #endregion

        private void Awake()
        {
            // Run if need to be selected on awake.
            if (_initOnAwake)
                SelectParameter(_parameterName);
        }
#if UNITY_EDITOR
        private void Update()
        {
            // Check debugger active, if not then ignore.
            if (!_debugControl) return;

            // Get default parameter from emitter.
            if (_selectedParameter == null)
                SelectParameter(_parameterName);

            // Run on first press.
            if (!Input.GetKeyDown(_pressKeyToRun)) return;

            // Check routine still running.
            if (_debugRuntimeRoutine != null)
            {
                // Stop routine.
                StopCoroutine(_debugRuntimeRoutine);
                _debugRuntimeRoutine = null;
            }

            // Create new routine.
            _debugRuntimeRoutine = DebugRuntimeRoutine();
            StartCoroutine(_debugRuntimeRoutine);
        }
#endif
        #region IRuntimeHandler

        public void OnPreRuntime()
        {
            // Change parameter value.
            _selectedParameter.Value = _preDefaultValue;
            _emitter.Play();

            // Call event.
            _onRunPreSound?.Invoke(_preDefaultValue);
        }

        public void OnRuntimeLoop()
        {
            // Change parameter value.
            _selectedParameter.Value = _loopDefaultValue;
            _emitter.Play();

            // Call event.
            _onRunSoundLoop?.Invoke(_loopDefaultValue);
        }

        public void OnPostRuntime()
        {
            // Change parameter value.
            _selectedParameter.Value = _postDefaultValue;
            _emitter.Play();

            // Call event.
            _onRunPostSound?.Invoke(_postDefaultValue);
        }

        public void OnPreRuntime(int invokerValue)
        {
            // Change parameter value.
            _selectedParameter.Value = invokerValue;
            _emitter.Play();

            // Call event.
            _onRunPreSound?.Invoke(invokerValue);
        }

        public void OnRuntimeLoop(int invokerValue)
        {
            // Change parameter value.
            _selectedParameter.Value = invokerValue;
            _emitter.Play();

            // Call event.
            _onRunSoundLoop?.Invoke(invokerValue);
        }

        public void OnPostRuntime(int invokerValue)
        {
            // Change parameter value.
            _selectedParameter.Value = invokerValue;
            _emitter.Play();

            // Call event.
            _onRunPostSound?.Invoke(invokerValue);
        }

        #endregion
#if FMOD
        #region Main

        /// <summary>
        /// To select parameter from emitter.
        /// </summary>
        /// <param name="parameterName">Select new default parameter.</param>
        public void SelectParameter(string parameterName = null)
        {
            // Set default parameter if need to be changed.
            if (!string.IsNullOrEmpty(parameterName))
                _parameterName = parameterName;

            // Get parameter reference if not cached yet.
            _selectedParameter = FindParam(_emitter, _parameterName);
        }

        private static ParamRef FindParam(StudioEventEmitter emitter, string parameterName)
        {
            // Find the parameter.
            int paramLen = emitter.Params.Length;
            for (int i = 0; i < paramLen; i++)
            {
                // Check if not the selected parameter, then skip it.
                if (emitter.Params[i].Name != parameterName)
                    continue;

                // Select parameter.
                return emitter.Params[i];
            }

            return null;
        }
#if UNITY_EDITOR
        private IEnumerator DebugRuntimeRoutine()
        {
            // Change parameter value and play it.
            _selectedParameter.Value = _preDefaultValue;
            _emitter.Play();

            // Wait until pre sound is not running.
            yield return new WaitUntil(() => !_emitter.IsPlaying());

            // Running debug logic, runtime loop.
            while (Input.GetKey(_pressKeyToRun))
            {
                // Change parameter value.
                _selectedParameter.Value = _loopDefaultValue;
                _emitter.Play();

                // Wait for one frame.
                yield return new WaitUntil(() => !_emitter.IsPlaying());
            }

            // Change parameter value.
            _selectedParameter.Value = _postDefaultValue;
            _emitter.Play();

            // Wait until pre sound is not running.
            yield return new WaitUntil(() => !_emitter.IsPlaying());

            // End of process.
            _debugRuntimeRoutine = null;
        }
#endif
        #endregion
#endif
    }
}
