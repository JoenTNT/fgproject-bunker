using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Enemy AI control.
    /// </summary>
    public class EnemyAIControl : BT_AIControl
    {
        #region Variables

        // Requirements
        [SerializeField]
        private Topview2DMovementFunc _movementFunc = null;

        [SerializeField]
        private Topview2DLookAtFunc _lookAtFunc = null;

        // Runtime variable data.
        private BakedDashboard _mainBakedDashboard = null;

        #endregion
#if UNITY_EDITOR
        #region Mono

        private void OnValidate()
        {
            // Always add dashboard.
            if (!TryGetComponent(out Dashboard dashboard))
                dashboard = gameObject.AddComponent<Dashboard>();
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
            _mainBakedDashboard.AssignValue<Component>(EnemyConstants.MOVEMENT_FUNCTION_KEY, _movementFunc);
            _mainBakedDashboard.AssignValue<Component>(EnemyConstants.LOOK_AT_FUNCTION_KEY, _lookAtFunc);

            // Always initialize base after dashboard data has been baked.
            base.OnInit();
        }

        #endregion
    }
}
