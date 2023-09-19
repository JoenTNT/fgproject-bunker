/* 
 * Copyright © 2023 JoenTNT All Rights Reserved
 * 
 * JT's Unity SOA Behaviour Tree
 * 
 * Sequence nodes execute children in order until one child returns Failure or all children returns Success.
 */

using UnityEngine;

namespace JT
{
    /// <summary>
    /// Handle sequencial AI process.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Behaviour Tree Sequence",
        menuName = "JT Framework/Behaviour Tree/BT Sequence")]
    public sealed class BT_Sequence : BT_Execute
    {

    }
}
