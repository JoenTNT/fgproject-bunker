using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Interactable component base.
    /// </summary>
    public abstract class InteractableComponent : MonoBehaviour, IInteractable<string>
    {
        #region Variable

        [Header("Requirements")]
        [SerializeField]
        private EntityID _uniqueID = null;

        #endregion

        #region Properties

        /// <summary>
        /// Notifier information.
        /// </summary>
        public abstract InteractionType Type { get; }

        /// <summary>
        /// Root of interactable entity ID.
        /// </summary>
        public string UniqueID => _uniqueID.ID;

        #endregion

        #region IInteractable

        public abstract bool Interact(string entity);

        #endregion
    }
}
