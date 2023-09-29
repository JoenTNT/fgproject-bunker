using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handles topdown movement 2d game.
    /// </summary>
    public class Topview2DMovementFunc : MonoBehaviour, IMovement2D
    {
        #region Variable

        [Header("Properties")]
        [SerializeField]
        private float _moveSpeed = 5f;

        [SerializeField]
        private bool _useRigidbody = false;

        [Header("Optional")]
        [SerializeField]
        private Rigidbody2D _rigidbody2D = null;

        // Runtime variable data
        private Vector2 _prevPos = Vector2.zero;
        private Vector2 _moveDir = Vector2.zero;

        #endregion

        #region Properties

        /// <summary>
        /// Calculated movement in one delta time.
        /// Data only available if not using Rigidbody.
        /// </summary>
        public Vector2 TickMove => _moveSpeed * _moveDir * Time.deltaTime;

        /// <summary>
        /// Previous position before running Move() method.
        /// Data only available if not using Rigidbody.
        /// </summary>
        public Vector2 BeforeTickPos => _prevPos;

        #endregion

        #region IMovement2D

        public float Speed
        {
            get => _moveSpeed;
            set => _moveSpeed = value;
        }

        public Vector2 Direction
        {
            get => _moveDir;
            set => _moveDir = value;
        }

        public void Move()
        {
            // Check if moving function uses rigidbody 2D.
            if (_useRigidbody)
            {
                _rigidbody2D.velocity = _moveDir * _moveSpeed;
                return;
            }

            // Use transform instead.
            _prevPos = transform.position;
            transform.position = _prevPos + TickMove;
        }

        #endregion
    }
}

