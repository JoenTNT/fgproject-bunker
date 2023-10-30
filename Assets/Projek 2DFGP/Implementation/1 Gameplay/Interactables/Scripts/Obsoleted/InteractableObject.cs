using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Entity can interact with.
    /// </summary>
    [System.Obsolete]
    public abstract class InteractableObject : MonoBehaviour, IInteractable<string>,
        IWaitUntilFinishHandler<string>, IProcessHandler<string>
    {
        #region Variable

        [Header("Interaction Settings")]
        [SerializeField]
        private bool _interruptable = false;

        // Temporary variable data
        private string _tempEntityID = string.Empty;
        private bool _isProcessing = false;

        #endregion

        #region Properties

        /// <summary>
        /// Which one of the interaction type of this object.
        /// </summary>
        public abstract InteractionType InteractType { get; }

        #endregion

        #region Mono

        private void Update()
        {
            // Check current processing.
            if (!_isProcessing) return;

            OnProcess(_tempEntityID);
        }

        #endregion

        #region IInteractable

        public bool IsInteractable => throw new System.NotImplementedException();

        public virtual bool Interact(string entityID)
        {
#if UNITY_EDITOR
            Debug.Log($"[DEBUG] Interact With Object: {this}");
#endif
            // Ignore when currently processing and not interruptable.
            if (_isProcessing && !_interruptable) return false;
            else if (_isProcessing && _interruptable) // If it's interruptable.
                Finish();

            _tempEntityID = entityID;
            _isProcessing = true;

            OnStartProcess(_tempEntityID);
            return true;
        }

        #endregion

        #region IWaitUntilFinishHandler

        public bool IsFinished => !_isProcessing;

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

            OnFinish?.Invoke(_tempEntityID);
            OnEndProcess(_tempEntityID);
            _tempEntityID = null;
            _isProcessing = false;
        }

        #endregion
    }
}
