using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// To check if enemy reach the targer before attack.
    /// </summary>
    public class EnemyReachTargetCondition : BT_Conditional
    {
        #region BT Conditional

        public override BT_State CalcStateCondition() => BT_State.Success;

        public override void OnInit()
        {
            
        }

        #endregion
    }
}
