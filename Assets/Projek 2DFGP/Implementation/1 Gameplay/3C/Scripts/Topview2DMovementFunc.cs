using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handles topdown movement 2d game.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Topview2DMovementFunc : MonoBehaviour, IMovement2D
    {
        #region Variable

        [SerializeField]
        private float _moveSpeed = 5f;

        // Temporary variable data
        private Rigidbody2D _rigidbody2D = null;
        private Vector2 _moveDir = Vector2.zero;

        #endregion

        #region Mono

        private void Awake() => TryGetComponent(out _rigidbody2D);

        #endregion

        #region IMovement2D

        public void Move(Vector2 dir) => _rigidbody2D.velocity = dir * _moveSpeed;

        #endregion
    }
}

