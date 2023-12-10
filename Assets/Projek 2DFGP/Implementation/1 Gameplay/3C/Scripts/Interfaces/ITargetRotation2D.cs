using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Targeting rotation for 2D Game.
    /// </summary>
    public interface ITargetRotation2D
    {
        /// <summary>
        /// Set look at position target.
        /// </summary>
        /// <param name="lookAtPos">Target position to look at</param>
        void SetTargetLookAtPosition(Vector2 lookAtPos);

        /// <summary>
        /// Set look direction target.
        /// </summary>
        /// <param name="lookDir">Target look direction</param>
        public void SetTargetLookDirection(Vector2 lookDir);

        /// <summary>
        /// Set rotation using Z degree.
        /// </summary>
        /// <param name="zDegree">Target rotation degree</param>
        public void SetTargetRotationDegree(float zDegree);

        /// <summary>
        /// Set rotation using Z radian.
        /// </summary>
        /// <param name="zRadian">Target rotation radian</param>
        public void SetTargetRotationRadian(float zRadian);
    }
}