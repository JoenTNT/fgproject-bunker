using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Bullet 2D object control.
    /// </summary>
    [RequireComponent(typeof(DestroyTimer))]
    public class Bullet2DControl : MonoBehaviour, IShootDirectionCommand<Vector2>
    {
        #region Variables

        [SerializeField]
        private Bullet2DControlData _data = new Bullet2DControlData();

        // Runtime variable data.
        private bool _isShoot = false;

        #endregion

        #region Mono

        private void Update()
        {
            // Check if bullet has been shoot.
            if (!_isShoot) return;

            // Move a bullet.
            _data.MoveFunc.Move();

            // Detect any target hit.
            _data.Hitter.OnDetectHit(transform.position, transform.forward, _data.MoveFunc.Speed * Time.deltaTime);
        }

        private void OnDisable() => _isShoot = false;

        #endregion

        #region IShootCommand

        public void Shoot(Vector2 direction, float speed)
        {
            // Set bullet movement information.
            _data.MoveFunc.Speed = speed;
            _data.MoveFunc.Direction = direction;
            _isShoot = true;

            // Bullet facing forward of the aim direction.
            _data.LookFunc.LookAtDirection(direction);
        }

        #endregion
    }

    /// <summary>
    /// Handle data for Bullet 2D Control.
    /// </summary>
    [System.Serializable]
    public class Bullet2DControlData
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private Topview2DMovementFunc _moveFunc = null;

        [SerializeField]
        private Topview2DLookAtFunc _lookFunc = null;

        [SerializeField]
        private Bullet2DHitter _hitter = null;

        #endregion

        #region Properties

        /// <summary>
        /// Function to move the bullet.
        /// </summary>
        public Topview2DMovementFunc MoveFunc => _moveFunc;

        /// <summary>
        /// Function to rotate bullet by shoot direction.
        /// </summary>
        public Topview2DLookAtFunc LookFunc => _lookFunc;

        /// <summary>
        /// Hitter handler.
        /// </summary>
        public Bullet2DHitter Hitter => _hitter;

        #endregion
    }
}
