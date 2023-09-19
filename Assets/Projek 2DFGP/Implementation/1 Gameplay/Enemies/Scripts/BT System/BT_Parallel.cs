/* 
 * Copyright © 2023 JoenTNT All Rights Reserved
 * 
 * JT's Unity SOA Behaviour Tree
 * 
 * Parallel nodes will execute all their children in “parallel”.
 * This is in quotes because it’s not true parallelism; at each tick, each child node will individually tick in order.
 * Parallel nodes return Success when at least M child nodes (between 1 and N) have succeeded, and Failure when all child nodes have failed.
 */

using UnityEngine;

namespace JT
{
    /// <summary>
    /// Running all actions behaviour at once.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Behaviour Tree Parallel",
        menuName = "JT Framework/Behaviour Tree/BT Parallel")]
    public sealed class BT_Parallel : BT_Execute
    {

    }
}
