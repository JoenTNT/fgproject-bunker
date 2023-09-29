/* 
 * Copyright © 2023 JoenTNT All Rights Reserved
 * 
 * JT's Unity SOA Behaviour Tree
 * 
 * Single abstract node to execute in runtime whether it is a condition or action node.
 */

using UnityEngine;

namespace JT
{
    /// <summary>
    /// Execute node in runtime.
    /// </summary>
    public abstract class BT_Execute : ScriptableObject, IBTState, IBTInitHandler
    {
        #region Variables

        // Runtime variable data.
        private GameObject _objectRef = null;
        private BT_State _currentState = BT_State.None;

        #endregion

        #region Properties

        /// <summary>
        /// Check if this is an action node.
        /// </summary>
        public abstract bool IsAction { get; }

        /// <summary>
        /// Owner of behaviour tree.
        /// </summary>
        protected GameObject ObjectRef => _objectRef;

        #endregion

        #region Main

        /// <summary>
        /// Execute function.
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Duplicate processor to distribute reference.
        /// </summary>
        public virtual BT_Execute GetCopy(GameObject objRef)
        {
            var copy = Instantiate(this);
            copy._objectRef = objRef;
            return copy;
        }

        #endregion

        #region IBTState

        public BT_State State
        {
            get => _currentState;
            protected set => _currentState = value;
        }

        #endregion

        #region IBTInitHandler

        /// <summary>
        /// Running when processor is initialized.
        /// </summary>
        public abstract void OnInit();

        #endregion
    }
}
