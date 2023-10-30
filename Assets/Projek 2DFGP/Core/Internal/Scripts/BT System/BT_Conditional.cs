/* 
 * Copyright © 2023 JoenTNT All Rights Reserved
 * 
 * JT's Unity SOA Behaviour Tree
 * 
 * Condition nodes can only return Success or Failure within a single tick.
 * This represents simple checks with only one checking procedure.
 */

using UnityEngine;

namespace JT
{
    /// <summary>
    /// A node to check condition validation.
    /// </summary>
    public abstract class BT_Conditional : BT_Execute, IBTConditional
    {
        #region BT Execute

        public sealed override bool IsAction => false;

        public sealed override void Execute() => State = CalcStateCondition();

        public sealed override BT_Execute GetCopy(GameObject objRef) => base.GetCopy(objRef);

        #endregion

        #region IBTConditional

        public abstract BT_State CalcStateCondition();

        #endregion
    }
}
