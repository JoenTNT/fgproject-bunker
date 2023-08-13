using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle using weapon state.
    /// </summary>
    public sealed class WeaponActionState : MonoBehaviour, IWeaponActionState
    {
        #region Variables

        [Header("Optional")]
        [SerializeField]
        private EntityID _owner = null;

        // Runtime variable data.
        private bool _isInAction = false;

        #endregion

        #region Properties

        /// <summary>
        /// Weapon belongs to.
        /// </summary>
        public EntityID Owner
        {
            get => _owner;
            set => _owner = value;
        }

        #endregion

        #region IWeaponState

        public bool IsInAction => _isInAction;

        public void StartAim() => _isInAction = true;

        public void Release() => _isInAction = false;

        #endregion
    }
}
