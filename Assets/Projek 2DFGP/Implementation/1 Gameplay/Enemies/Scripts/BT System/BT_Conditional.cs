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
    public abstract class BT_Conditional : BT_Execute, IBTValidateCondition
    {
        #region IBTValidateCondition

        public abstract bool IsConditionValid();

        #endregion
    }
}
