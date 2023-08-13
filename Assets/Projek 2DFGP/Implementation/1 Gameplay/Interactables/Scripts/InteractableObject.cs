using System.Collections;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Entity can interact with.
    /// </summary>
    public abstract class InteractableObject : MonoBehaviour, IInteractable<string>,
        IWaitUntilFinishHandler<string>, IProcessHandler<string>
    {
        #region Variable

        [Header("Interaction Settings")]
        [SerializeField]
        private bool _interruptable = false;

        // Temporary variable data
        private IEnumerator _currentRunningProcess = null;
        private bool _isProcessing = false;
        private bool _isFinished = false;

        #endregion

        #region Properties

        /// <summary>
        /// Which one of the interaction type of this object.
        /// </summary>
        public abstract InteractionType InteractType { get; }

        #endregion

        #region IInteractable

        public bool Interact(string entityID)
        {
#if UNITY_EDITOR
            Debug.Log($"Interact With Object: {this}");
#endif
            if (_currentRunningProcess != null)
            {
                if (!_interruptable) return false;
                StopCoroutine(_currentRunningProcess);
            }

            _isProcessing = true;
            _currentRunningProcess = AwaitProcess(entityID);
            StartCoroutine(_currentRunningProcess);
            return true;
        }

        #endregion

        #region IWaitUntilFinishHandler

        public bool IsFinished => _isFinished;

        public event System.Action<string> OnFinish;

        #endregion

        #region IProcessHandler

        public bool IsProcessing => _isProcessing;

        public bool ProcessInterruptable
        {
            get => _interruptable;
            set => _interruptable = value;
        }

        public virtual void OnStartProcess(string entityID) { }

        public virtual void OnProcess(string entityID) { }

        public virtual void OnEndProcess(string entityID) { }

        #endregion

        #region Main

        /// <summary>
        /// Run this when the process has finished.
        /// </summary>
        protected void Finish()
        {
            if (!_isProcessing) return;

            _isProcessing = false;
            _isFinished = true;
        }

        private IEnumerator AwaitProcess(string entityID)
        {
            OnStartProcess(entityID);

            while (_isProcessing)
            {
                OnProcess(entityID);
                yield return null;
            }

            OnEndProcess(entityID);

            _currentRunningProcess = null;
            OnFinish?.Invoke(entityID);
        }

        #endregion
    }
}
