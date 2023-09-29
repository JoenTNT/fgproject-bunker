using JT.FGP;
using UnityEngine;

namespace JT
{
    // Shorten includes.
    using EC = EnemyConstants;

    /// <summary>
    /// Handle enemy moving by walk action.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Enemy Walking Action",
        menuName = "FGP/AI/Enemy Walking Action")]
    public class EnemyWalkingAction : BT_Action
    {
        #region Variable

        // Initial variable references.
        private BakedDashboard _dashboard = null;
        private BakedParamTransform _targetParam = null;

        #endregion

        #region BT Action

        public override BT_State CalcStateCondition()
        {
            // Check if current target chase.
            return _targetParam.Value == null ? BT_State.Success : BT_State.Failed;
        }

        public override void OnInit()
        {
            // Get target dashboard.
            _dashboard = GlobalDPContainer.GetDashboard(ObjectRef.GetInstanceID());

            // Get all references.
            var @params = _dashboard.Parameters;
            _targetParam = (BakedParamTransform)@params[EC.CHASE_TARGET_KEY];
        }

        #endregion
    }
}

