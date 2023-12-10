/* 
 * Copyright © 2023 JoenTNT All Rights Reserved
 * 
 * JT's Unity SOA Behaviour Tree
 * 
 * Action nodes can span multiple ticks and can return Running until they reach a terminal state.
 * This represents complex action that will be running every tick update.
 */

using UnityEngine;

namespace JT
{
    /// <summary>
    /// Behaviour tree action.
    /// </summary>
    public abstract class BT_Action : BT_Execute, IBTRuntimeAction
    {
        #region BT Execute

        public sealed override bool IsAction => true;

        public sealed override void Execute() => State = BT_State.Running;

        public sealed override BT_Execute GetCopy(GameObject objRef) => base.GetCopy(objRef);

        #endregion

        #region IBTRuntimeAction

        public bool IsDone => State != BT_State.Running;

        public virtual void OnAfterAction() { }

        public virtual void OnBeforeAction() { }

        public virtual void OnTickAction() { }

        #endregion
    }
}
