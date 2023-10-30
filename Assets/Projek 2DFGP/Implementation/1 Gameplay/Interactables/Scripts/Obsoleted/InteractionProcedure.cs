using UnityEngine;
using UnityEngine.Events;

namespace JT.FGP
{
    /// <summary>
    /// Interaction group control.
    /// </summary>
    [System.Obsolete]
    public class InteractionProcedure : MonoBehaviour, IInteractable<string>, ILocker,
        IInteractionHandler
    {
        #region Variable

        [SerializeField]
        private InteractableObject[] _interactions = new InteractableObject[0];

        [SerializeField]
        private EntityID _entityID = null;

        [SerializeField]
        private bool _lockOnStart = false;

        [SerializeField]
        private bool _unlimitedInteraction = true;

        [SerializeField]
        private UnityEvent _onInteractionStart = new UnityEvent();

        [SerializeField]
        private UnityEvent _onInteractionEnded = new UnityEvent();

        // Temporary variable data
        private int _currentInteractionIndex = 0;
        private bool _isInteractionLocked = false;

        #endregion

        #region Properties

        /// <summary>
        /// Interaction mark ID.
        /// </summary>
        public string ID => _entityID.ID;

        /// <summary>
        /// Is the interaction can be called infinitely.
        /// </summary>
        public bool InfiniteInteraction
        {
            get => _unlimitedInteraction;
            set => _unlimitedInteraction = value;
        }

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events
            for (int i = 0; i < _interactions.Length; i++)
                if (_interactions[i] != null)
                    _interactions[i].OnFinish += ListeonOnInteractionObjectFinish;
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            for (int i = 0; i < _interactions.Length; i++)
                if (_interactions[i] != null)
                    _interactions[i].OnFinish -= ListeonOnInteractionObjectFinish;
        }

        private void Start() => _isInteractionLocked = _lockOnStart;

        #endregion

        #region IInteractable

        public bool IsInteractable => throw new System.NotImplementedException();

        public bool Interact(string entityID)
        {
            if (_isInteractionLocked) return false;
            if (!_unlimitedInteraction) Lock();
#if UNITY_EDITOR
            //Debug.Log($"Start Interaction Procedure: {this}");
#endif
            OnInteractionStart();

            // Find starting interaction with entity
            for (_currentInteractionIndex = 0; _currentInteractionIndex < _interactions.Length; _currentInteractionIndex++)
            {
                if (_interactions[_currentInteractionIndex] == null) continue;

                _interactions[_currentInteractionIndex].Interact(entityID);
                break;
            }

            return true;
        }

        #endregion

        #region ILocker

        public bool IsLocked => _isInteractionLocked;

        public void Lock() => _isInteractionLocked = true;

        public void Unlock() => _isInteractionLocked = false;

        #endregion

        #region IInteractionHandler

        public virtual void OnInteractionStart() => _onInteractionStart?.Invoke();

        public virtual void OnInteractionEnded() => _onInteractionEnded?.Invoke();

        #endregion

        #region Main

        private void ListeonOnInteractionObjectFinish(string withEntity)
        {
            // Find next non null interaction
            for (_currentInteractionIndex++; _currentInteractionIndex < _interactions.Length;
                _currentInteractionIndex++)
            {
                if (_interactions[_currentInteractionIndex] == null) continue;
                break;
            }

            if (_currentInteractionIndex >= _interactions.Length)
            {
                OnInteractionEnded();
                return;
            }

            _interactions[_currentInteractionIndex].Interact(withEntity);
        }

        #endregion
    }
}
