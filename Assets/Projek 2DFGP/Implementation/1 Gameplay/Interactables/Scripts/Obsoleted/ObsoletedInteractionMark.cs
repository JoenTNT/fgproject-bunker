using JT.GameEvents;
using System.Collections.Generic;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// The main interaction mark which player can interact with.
    /// </summary>
    [System.Obsolete]
    [RequireComponent(typeof(Collider2D))]
    public sealed class ObsoletedInteractionMark : MonoBehaviour, IInteractable<string>, ILocker
    {
        #region Variable

        [Header("Properties")]
        [SerializeField]
        private EntityID _entityID = null;

        [SerializeField]
        private InteractionType _interactionDisplay = InteractionType.Interact;

        [SerializeField]
        private bool _lockOnAwake = false;

        [SerializeField]
        private InteractionProcedure[] _targetRunInteracts = new InteractionProcedure[0];

        [Header("Game Events")]
        [SerializeField]
        private GameEventStringBool _setLockInteractionMarkCallback = null;

        [SerializeField]
        private GameEventTwoStringBool _setLockInteractionProcedureCallback = null;

        // Temporary variable data
        private Dictionary<string, InteractionProcedure> _interactableDictionary = new();
        private bool _isMarkerLocked = false;

        #endregion

        #region Properties

        /// <summary>
        /// The interaction type marker that is shown.
        /// </summary>
        public InteractionType DisplayTypeInteraction => _interactionDisplay;

        #endregion

        #region Mono

        private void Awake()
        {
            InitDictionary();

            _isMarkerLocked = _lockOnAwake;

            // Subscribe events
            _setLockInteractionMarkCallback.AddListener(ListenSetLockInteractionMarkCallback);
            _setLockInteractionProcedureCallback.AddListener(ListenSetLockInteractionProcedureCallback);
        }

        private void OnDestroy()
        {
            // Unsubscribe events
            _setLockInteractionMarkCallback.RemoveListener(ListenSetLockInteractionMarkCallback);
            _setLockInteractionProcedureCallback.RemoveListener(ListenSetLockInteractionProcedureCallback);
        }

        #endregion

        #region IInteractable

        public bool IsInteractable => throw new System.NotImplementedException();

        public bool Interact(string entityID)
        {
            if (_isMarkerLocked)
            {
#if UNITY_EDITOR
                Debug.Log($"The Marker Is Locked, Unable to Interact with {_entityID.ID}");
#endif
                return false;
            }

            foreach (var i in _interactableDictionary)
                i.Value.Interact(entityID);
            return true;
        }

        #endregion

        #region ILocker

        public bool IsLocked => _isMarkerLocked;

        public void Lock() => _isMarkerLocked = true;

        public void Unlock() => _isMarkerLocked = false;

        #endregion

        #region Main

        private void ListenSetLockInteractionMarkCallback(string markID, bool isLock)
        {
            if (_entityID.ID != markID) return;

            if (isLock) Lock();
            else Unlock();
        }

        private void ListenSetLockInteractionProcedureCallback(string markID, string procedureID, bool isLock)
        {
            if (_entityID.ID != markID) return;
            if (!_interactableDictionary.ContainsKey(procedureID)) return;

            if (isLock) _interactableDictionary[procedureID].Lock();
            else _interactableDictionary[procedureID].Unlock();
        }

        /// <summary>
        /// Initialize dictionary for better search algorithm.
        /// </summary>
        private void InitDictionary()
        {
            if (_interactableDictionary == null)
                _interactableDictionary = new();

            for (int i = 0; i < _targetRunInteracts.Length; i++)
            {
                if (_targetRunInteracts[i] == null) continue;

                _interactableDictionary[_targetRunInteracts[i].ID] = _targetRunInteracts[i];
            }
        }

        #endregion
    }
}
