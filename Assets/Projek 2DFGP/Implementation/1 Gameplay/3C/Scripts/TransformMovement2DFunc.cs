using UnityEngine;

namespace JT.FGP
{
    // TEMP: For topdown movement only.
    /// <summary>
    /// Handle transform translation movement for 2D entity.
    /// </summary>
    public class TransformMovement2DFunc : AbstractMovement2DFunc, IDirectionBaseMovement2D
    {
        #region Variables

        // Runtime variable data.
        private Vector2 _direction = Vector2.zero;

        #endregion

        #region AbstractMovement2DFunc

        public override Vector2 Velocity => _direction * SpeedMultiplier;

        public override void OnMove()
        {
            Vector2 currentPos = transform.position;
            currentPos += _direction * SpeedMultiplier * Time.deltaTime;
            transform.position = currentPos;
        }

        #endregion

        #region IDirectionBaseMovement2D

        public void SetMoveDirection(Vector2 targetDir) => _direction = targetDir;

        #endregion
    }
}
