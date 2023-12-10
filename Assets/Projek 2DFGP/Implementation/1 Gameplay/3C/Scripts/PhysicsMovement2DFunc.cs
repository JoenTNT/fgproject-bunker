using UnityEngine;

namespace JT.FGP
{
    // TEMP: For topdown movement only.
    /// <summary>
    /// Handle physics or rigidbody movement for 2D entity.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PhysicsMovement2DFunc : AbstractMovement2DFunc, IDirectionBaseMovement2D
    {
        #region Variables

        // Runtime variable data.
        private Rigidbody2D _rb2D = null;
        private Vector2 _direction = Vector2.zero;

        #endregion

        #region Mono

        private void Awake()
        {
            // Set all initial references.
            TryGetComponent(out _rb2D);
        }

        #endregion

        #region AbstractMovement2DFunc

        public override Vector2 Velocity => _rb2D.velocity;

        public override void OnMove() => _rb2D.velocity = _direction * SpeedMultiplier;

        #endregion

        #region IDirectionBaseMovement2D

        public void SetMoveDirection(Vector2 targetDir) => _direction = targetDir;

        #endregion

        #region Main

        /// <summary>
        /// Stop the movement process.
        /// </summary>
        public void StopMovement() => _rb2D.velocity = Vector2.zero;

        #endregion
    }
}
