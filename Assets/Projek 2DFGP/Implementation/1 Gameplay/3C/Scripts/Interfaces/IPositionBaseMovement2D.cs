using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Set movement target base on position for 2D game.
    /// </summary>
    public interface IPositionBaseMovement2D
    {
        /// <summary>
        /// Is target position has been reached?
        /// </summary>
        bool HasReachedDestination { get; }

        /// <summary>
        /// Set movement base on target position.
        /// </summary>
        /// <param name="targetPos"></param>
        void SetMoveToPosition(Vector2 targetPos);
    }
}
