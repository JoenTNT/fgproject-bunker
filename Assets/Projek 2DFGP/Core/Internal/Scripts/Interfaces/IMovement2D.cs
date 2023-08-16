using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle movement in 2D game.
    /// </summary>
    public interface IMovement2D
    {
        /// <summary>
        /// Move direction.
        /// </summary>
        Vector2 Direction { get; set; }

        /// <summary>
        /// Speed of movement.
        /// </summary>
        float Speed { get; set; }

        /// <summary>
        /// Move method, run this in update.
        /// </summary>
        void Move();
    }
}
