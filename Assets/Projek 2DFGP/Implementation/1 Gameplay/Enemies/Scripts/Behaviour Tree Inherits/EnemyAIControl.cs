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
        private EntityID _ownerID = null;

        [SerializeField]
        private NavMeshAgent _agent = null;

        [SerializeField]
        private HitpointStats _hitpointStats = null;

        [SerializeField]
        private AbstractMovement2DFunc _movementFunc = null;

        [SerializeField]
        private DampingRotation2DFunc _lookAtFunc = null;

        [Header("Optional")]
        [SerializeField]
        private InsideAreaObjectCollector2D _objectCollector2D = null;

        [SerializeField]
        private AbsoluteFollowObject2DFunc _followObject2DFunc = null;

        [SerializeField]
        private Shooter2DFunc _shooterFunc = null;

        [SerializeField]
        private Animator _animator = null;

        // Runtime variable data.
        private BakedDashboard _bakedDashboard = null;

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
            _bakedDashboard = mainDashboard.Bake();
            _bakedDashboard.AssignValue<Component>(EC.MOVEMENT_FUNCTION_KEY, _movementFunc);
            _bakedDashboard.AssignValue<Component>(EC.ROTATION_FUNCTION_KEY, _lookAtFunc);
            _bakedDashboard.AssignValue<Component>(EC.NAVMESH_AGENT_KEY, _agent);
            _bakedDashboard.AssignValue<Component>(EC.INSIDE_FOV_AREA_KEY, _objectCollector2D);
            _bakedDashboard.AssignValue<Component>(EC.HITPOINT_STATS_KEY, _hitpointStats);
            _bakedDashboard.AssignValue<Component>(EC.SHOOTER_FUNCTION_KEY, _shooterFunc);
            _bakedDashboard.AssignValue<Component>(EC.OWNER_ID_KEY, _ownerID);
            _bakedDashboard.AssignValue<Component>(EC.ANIMATOR_KEY, _animator);

            // Always initialize base after dashboard data has been baked.
            base.OnInit();
        }
#if UNITY_EDITOR
        
        protected override void RunInitOnEditor()
        {
            // Always add dashboard.
            if (!TryGetComponent(out Dashboard dashboard)) return;
            if (Root == null) return;

            var keys = Root.GetVariableKeys();
            keys[EC.MOVEMENT_FUNCTION_KEY] = typeof(ParamComponent).AssemblyQualifiedName;
            keys[EC.ROTATION_FUNCTION_KEY] = typeof(ParamComponent).AssemblyQualifiedName;
            keys[EC.NAVMESH_AGENT_KEY] = typeof(ParamComponent).AssemblyQualifiedName;
            keys[EC.INSIDE_FOV_AREA_KEY] = typeof(ParamComponent).AssemblyQualifiedName;
            keys[EC.HITPOINT_STATS_KEY] = typeof(ParamComponent).AssemblyQualifiedName;
            keys[EC.SHOOTER_FUNCTION_KEY] = typeof(ParamComponent).AssemblyQualifiedName;
            keys[EC.OWNER_ID_KEY] = typeof(ParamComponent).AssemblyQualifiedName;
            keys[EC.ANIMATOR_KEY] = typeof(ParamComponent).AssemblyQualifiedName;
            dashboard.RegisterAllKeys(keys);
        }
#endif
        #endregion
    }
}
