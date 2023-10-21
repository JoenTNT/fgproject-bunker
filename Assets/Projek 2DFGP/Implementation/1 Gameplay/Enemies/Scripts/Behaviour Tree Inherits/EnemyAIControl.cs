using JT.GameEvents;
using UnityEngine;
using UnityEngine.AI;

namespace JT.FGP
{
    // Shorten includes.
    using EC = EnemyConstants;

    /// <summary>
    /// Enemy AI control.
    /// </summary>
    public class EnemyAIControl : BT_AIControl
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private NavMeshAgent _agent = null;

        [SerializeField]
        private HitpointStats _hitpointStats = null;

        [SerializeField]
        private Topview2DMovementFunc _movementFunc = null;

        [SerializeField]
        private Topview2DLookAtFunc _lookAtFunc = null;

        [SerializeField]
        private InsideAreaObjectCollector2D _objectCollector2D = null;

        [Header("Optional")]
        [SerializeField]
        private AbsoluteFollowObject2DFunc _followObject2DFunc = null;

        // Runtime variable data.
        private BakedDashboard _mainBakedDashboard = null;

        #endregion

#if UNITY_EDITOR
        #region Mono

        private void OnValidate()
        {
            // Always add dashboard.
            if (!TryGetComponent(out Dashboard _))
                gameObject.AddComponent<Dashboard>();
        }

        #endregion
#endif
        #region BT AIControl

        public override void OnInit()
        {
            // Check for implemented dashboard, if not then abort process.
            if (!TryGetComponent(out Dashboard mainDashboard)) return;

            // Bake dashboard.
            _mainBakedDashboard = mainDashboard.Bake();
            _mainBakedDashboard.AssignValue<Component>(EC.MOVEMENT_FUNCTION_KEY, _movementFunc);
            _mainBakedDashboard.AssignValue<Component>(EC.LOOK_AT_FUNCTION_KEY, _lookAtFunc);
            _mainBakedDashboard.AssignValue<Component>(EC.NAVMESH_AGENT_KEY, _agent);
            _mainBakedDashboard.AssignValue<Component>(EC.INSIDE_FOV_AREA_KEY, _objectCollector2D);
            _mainBakedDashboard.AssignValue<Component>(EC.HITPOINT_STATS_KEY, _hitpointStats);

            // Always initialize base after dashboard data has been baked.
            base.OnInit();
        }

        #endregion
    }
}
