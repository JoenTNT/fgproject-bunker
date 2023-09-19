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
    public abstract class BT_Action : BT_Execute, IBTRuntimeAction
    {
        #region IBTRuntimeAction

        public virtual void OnAfterAction() { }

        public virtual void OnBeforeAction() { }

        public virtual void OnTickAction() { }

        #endregion
    }
}
