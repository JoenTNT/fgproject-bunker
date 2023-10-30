using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Check if there's no target around.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Enemy Has Target Condition",
        menuName = "FGP/AI/Enemy Has Target Condition")]
    public class EnemyHasTargetCondition : BT_Conditional
    {
        #region Variables

        // Runtime variable data.
        private BakedDashboard _dashboard = null;
        private BakedParamTransform _targetParam = null;

        #endregion

        #region BT Conditional

        public override BT_State CalcStateCondition()
        {
            // Check if current target chase.
            return _targetParam.Value != null ? BT_State.Success : BT_State.Failed;
        }

        public override void OnInit()
        {
            // Get dashboard.
            _dashboard = GlobalDPContainer.GetDashboard(ObjectRef.GetInstanceID());

            // Get all references.
            _targetParam = (BakedParamTransform)_dashboard.Parameters[EnemyConstants.CHASE_TARGET_KEY];
        }

        #endregion
    }
}
