using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle using weapon state.
    /// </summary>
    public sealed class WeaponActionState : MonoBehaviour, IWeaponActionState
    {
        #region Variables

        // Runtime variable data.
        private string _ownerOnState = null;
        private bool _isInAction = false;

        #endregion

        #region IWeaponState

        public event System.Action OnInstantUse;

        public bool IsInAction => _isInAction;

        public string OwnerOfState
        {
            get => _ownerOnState;
            set => _ownerOnState = value;
        }

        public void StartAim() => _isInAction = true;

        public void Release() => _isInAction = false;

        public void InstantUse() => OnInstantUse?.Invoke();

        #endregion
    }
}
