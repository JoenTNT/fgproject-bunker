using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle attacking target.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Enemy Attacking Action",
        menuName = "FGP/AI/Enemy Attacking Action")]
    public class EnemyAttackAction : BT_Action
    {
        #region BT Action

        public override BT_State CalcStateCondition()
        {
            return BT_State.Success;
        }

        public override void OnInit()
        {
            
        }

        #endregion
    }
}

