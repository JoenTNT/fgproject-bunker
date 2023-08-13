using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle movement in 2D game.
    /// </summary>
    public interface IMovement2D
    {
        /// <summary>
        /// Move method, run this in update.
        /// </summary>
        /// <param name="dir">Move direction</param>
        void Move(Vector2 dir);
    }
}
