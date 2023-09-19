/* 
 * Copyright © 2023 JoenTNT All Rights Reserved
 * 
 * JT's Unity SOA Behaviour Tree
 * 
 * Fallback nodes execute children in order until one of them returns Success or all children return Failure.
 * These nodes are key in designing recovery behaviors for your autonomous agents.
 */

using UnityEngine;

namespace JT
{
    /// <summary>
    /// Handle behaviour tree condition checker fallback.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Behaviour Tree Fallback",
        menuName = "JT Framework/Behaviour Tree/BT Fallback")]
    public sealed class BT_Fallback : BT_Execute
    {

    }
}
