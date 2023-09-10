using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle data for stats control data.
    /// </summary>
    [System.Serializable]
    public class StatsControlData
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private EntityID _entityID = null;

        [SerializeField]
        private HitpointStats _hpStats = null;

        #endregion

        #region Properties

        /// <summary>
        /// Entity ID of an entity.
        /// </summary>
        public string EntityID => _entityID.ID;

        /// <summary>
        /// Hitpoint stats.
        /// </summary>
        public IHitPoint<float> HP => _hpStats;

        #endregion
    }
}

