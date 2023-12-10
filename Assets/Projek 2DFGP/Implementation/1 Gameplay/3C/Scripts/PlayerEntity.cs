using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Player entity runtime main data.
    /// </summary>
    [RequireComponent(typeof(EntityID))]
    public class PlayerEntity : MonoBehaviour, IInteractable
    {
        #region Variable

        [Header("Properties")]
        [SerializeField]
        private PlayerEntityData _data = null;

        [Header("Game Events")]
        [SerializeField]
        private GameEventTwoStringInt _notifyInteractionHintCallback = null;

        [SerializeField]
        private GameEventStringUnityObject _onAddRuntimePlayerInfo = null;

        // Runtime variable data
        private System.Func<GameObject, bool> _isInteractableFunc = null;
        private GameObject _nearestDetectedInteractable = null;
        private InteractableComponent _nearestInteractable = null;
        private InteractableComponent _tempInteractable = null;

        #endregion

        #region Properties

        /// <summary>
        /// Player entity ID.
        /// </summary>
        public string EntityID => _data.ID;

        #endregion

        #region Mono

        private void Awake()
        {
            // Inject data to controller.
            if (TryGetComponent(out PlayerControl controller))
                controller.InjectEntityData(_data);

            // Set initial data.
            _isInteractableFunc = IsInteractableFunc;
        }

        private void Start()
        {
            // Initialize all data on start.
            _data.Initialize();

            // Send runtime stats to Canvas.
            _onAddRuntimePlayerInfo.Invoke(_data.ID, _data.RuntimeStatsRef);
        }

        private void Update() => HandleSearchNearestInteractable();

        #endregion

        #region IInteractable

        public bool IsInteractable => !_data.IsInteracting;

        public bool Interact()
        {
            if (_nearestInteractable == null) return false;
#if UNITY_EDITOR
            //Debug.Log($"[DEBUG] Interact with {_nearestInteractable}");
#endif
            _nearestInteractable.EntityObjectTarget = gameObject;
            bool result = _nearestInteractable.Interact(_data.ID);
            if (!_nearestInteractable.IsInteractable)
            {
                // Remove the current and change with new one.
                _nearestDetectedInteractable = null;
                HandleSearchNearestInteractable();
            }
            else
            {
                // Update information of interactable case.
                _notifyInteractionHintCallback.Invoke(_data.ID, _nearestInteractable.ID,
                    (int)_nearestInteractable.Type);
            }
            return result;
        }

        #endregion

        #region Main

        private void HandleSearchNearestInteractable()
        {
            // Handle interaction control
            _nearestDetectedInteractable = _data.AreaOfInteraction.GetNearestObject(
                transform.position, _isInteractableFunc);
            if (_nearestDetectedInteractable != null)
            {
                if (_nearestInteractable?.gameObject == _nearestDetectedInteractable) return;
                if (!_nearestDetectedInteractable.TryGetComponent(out _nearestInteractable)) return;

                _notifyInteractionHintCallback.Invoke(_data.ID, _nearestInteractable.ID,
                    (int)_nearestInteractable.Type);
            }
            else
            {
                if (_nearestInteractable == null) return;

                _notifyInteractionHintCallback.Invoke(_data.ID, null, (int)InteractionType.None);
                _nearestInteractable = null;
            }
        }

        private bool IsInteractableFunc(GameObject interactableObj)
        {
            if (interactableObj.TryGetComponent(out _tempInteractable))
                return _tempInteractable.IsInteractable;
            return true;
        }

        #endregion
    }
}
