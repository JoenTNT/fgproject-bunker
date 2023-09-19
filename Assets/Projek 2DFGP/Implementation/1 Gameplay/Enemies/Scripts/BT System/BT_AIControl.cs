/* 
 * Copyright © 2023 JoenTNT All Rights Reserved
 * 
 * JT's Unity SOA Behaviour Tree
 * 
 * Main control of behaviour tree.
 */

using UnityEngine;

namespace JT
{
    /// <summary>
    /// Behaviour trees system control.
    /// </summary>
    public abstract class BT_AIControl : MonoBehaviour, IBTActivation
    {
        #region IBTActivation

        public virtual void OnStartBehaviour() { }

        public virtual void OnEndBehaviour() { }
        
        public virtual void OnEnableBehaviour() { }

        public virtual void OnDisableBehaviour() { }

        #endregion
    }
}
