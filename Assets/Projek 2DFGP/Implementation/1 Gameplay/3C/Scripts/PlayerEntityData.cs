using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle data for player entity.
    /// </summary>
    [System.Serializable]
    internal class PlayerEntityData : IRequiredInitialize
    {
        #region Variable

        [Header("Requirements")]
        [SerializeField]
        private EntityID _entityID = null;

        [SerializeField]
        private HitpointStats _physicalStats = null;

        [SerializeField]
        private UI_RuntimePlayerStats _uiProfilePrefab = null;

        [SerializeField]
        private InsideAreaObjectCollector2D _areaOfWeaponSight = null;

        [SerializeField]
        private InsideAreaObjectCollector2D _areaOfInteraction = null;

        // Runtime variable data.
        private IWeaponActionState _weaponOnHand = null;
        private UI_RuntimePlayerStats _runtimeStatsRef = null;
        private bool _isInteracting = false;
        private bool _isInitialized = false;

        #endregion

        #region Properties

        /// <summary>
        /// Entity identifier.
        /// </summary>
        public string ID => _entityID.ID;

        /// <summary>
        /// Player see target to aim.
        /// </summary>
        public InsideAreaObjectCollector2D AreaOfWeaponSight => _areaOfWeaponSight;

        /// <summary>
        /// Player interaction area.
        /// </summary>
        public InsideAreaObjectCollector2D AreaOfInteraction => _areaOfInteraction;

        /// <summary>
        /// Access UI runtime stats reference from UI Canvas.
        /// </summary>
        public UI_RuntimePlayerStats RuntimeStatsRef => _runtimeStatsRef;

        /// <summary>
        /// Weapon on player's hand.
        /// This can be null because player may not equip any weapon.
        /// </summary>
        public IWeaponActionState Weapon
        {
            get => _weaponOnHand;
            set
            {
                // Unset old one.
                if (_weaponOnHand != null)
                    _weaponOnHand.Owner = string.Empty;

                // Set weapon ownership if not null.
                if (value != null)
                    value.Owner = _entityID.ID;
                _weaponOnHand = value;
            }
        }

        /// <summary>
        /// Player status if currently interacting with something and cannot be controlled.
        /// </summary>
        public bool IsInteracting
        {
            get => _isInteracting;
            set => _isInteracting = value;
        }

        #endregion

        #region IRequiredInitialize

        public bool IsInitialized => _isInitialized;

        public void Initialize()
        {
            // Check initialized status.
            if (_isInitialized) return;

            // Initialize everything.
            _runtimeStatsRef = Object.Instantiate(_uiProfilePrefab);
            _physicalStats.InjectBarRef(_runtimeStatsRef.HealthBar);

            // Set status cleared.
            _isInitialized = true;
        }

        #endregion
    }
}