using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Interactable component base.
    /// </summary>
    public abstract class InteractableComponent : MonoBehaviour, IInteractable<string>,
        IEntityObjectTarget<GameObject>
    {
        #region Variable

        [SerializeField]
        private string _id = null;

        // Runtime variable data.
        private GameObject _latestEntityObject = null;
        private bool _isInteractable = true;

        #endregion

        #region Properties

        /// <summary>
        /// Notifier information.
        /// </summary>
        public abstract InteractionType Type { get; }

        /// <summary>
        /// Root of interactable ID.
        /// </summary>
        public string ID
        {
            get => _id;
            set => _id = value;
        }

        #endregion

        #region IInteractable

        public bool IsInteractable
        {
            get => _isInteractable;
            protected set => _isInteractable = value;
        }

        public abstract bool Interact(string entity);

        #endregion

        #region IEntityObjectTarget

        public GameObject EntityObjectTarget
        {
            get => _latestEntityObject;
            set => _latestEntityObject = value;
        }

        #endregion
    }
}
