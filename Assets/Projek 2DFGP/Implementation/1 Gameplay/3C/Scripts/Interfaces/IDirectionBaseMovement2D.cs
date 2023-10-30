using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Set movement target base on direction for 2D game.
    /// </summary>
    public interface IDirectionBaseMovement2D
    {
        /// <summary>
        /// Set movement base on target direction.
        /// </summary>
        /// <param name="targetDir">Move direction, no need to be normalized</param>
        void SetMoveDirection(Vector2 targetDir);
    }
}
