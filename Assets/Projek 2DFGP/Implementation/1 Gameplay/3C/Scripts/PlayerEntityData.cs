using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle data for player entity.
    /// </summary>
    [System.Serializable]
    internal class PlayerEntityData
    {
        #region Variable

        [Header("Requirements")]
        [SerializeField]
        private EntityID _entityID = null;

        [SerializeField]
        private InsideAreaObjectCollector2D _areaOfWeaponSight = null;

        [SerializeField]
        private InsideAreaObjectCollector2D _areaOfInteraction = null;

        // Runtime variable data.
        private IWeaponActionState _weaponOnHand = null;

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
                    _weaponOnHand.OwnerOfState = string.Empty;

                // Set weapon ownership if not null.
                if (value != null)
                    value.OwnerOfState = _entityID.ID;
                _weaponOnHand = value;
            }
        }

        #endregion
    }
}