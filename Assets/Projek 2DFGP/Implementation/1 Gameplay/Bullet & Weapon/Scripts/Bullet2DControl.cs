using UnityEngine;
using FMODUnity;

namespace JT.FGP
{
    /// <summary>
    /// Bullet 2D object control.
    /// </summary>
    [RequireComponent(typeof(DestroyTimer))]
    public class Bullet2DControl : MonoBehaviour, IShootDirectionCommand<Vector2>, IRequiredReset
    {
        #region Variables

        [SerializeField]
        private Bullet2DControlData _data = new Bullet2DControlData();

        // Runtime variable data.
        private DestroyTimer _destroyTimer = null;
        private string _shooterID = string.Empty;
        private bool _isShoot = false;

        #endregion

        #region Properties

        /// <summary>
        /// The shooter ID.
        /// </summary>
        public string ShooterID
        {
            get => _shooterID;
            set => _shooterID = value;
        }

        #endregion

        #region Mono

        private void FixedUpdate()
        {
            // Check if bullet has been shoot.
            if (!_isShoot) return;

            // Detect any target hit.
            _data.Hitter.OnDetectHit(transform.position, _data.MoveFunc.Direction,
                _data.MoveFunc.Speed * Time.deltaTime);

            // Move a bullet.
            _data.MoveFunc.Move();
        }

        private void OnDisable() => _isShoot = false;

        #region IRequiredReset

        public void Reset()
        {
            // Get destroy timer component.
            if (_destroyTimer == null)
                TryGetComponent(out _destroyTimer);

            // Reset the timer and the bullet hitter.
            _destroyTimer.Reset();
            _data.Hitter.Reset();
        }

        #endregion

        #endregion

        #region IShootDirectionCommand

        public void Shoot(Vector2 direction, float speed)
        {
            // Set bullet movement information.
            _data.MoveFunc.Speed = speed;
            _data.MoveFunc.Direction = direction;
            _isShoot = true;

            // Bullet facing forward of the aim direction.
            _data.LookFunc.LookAtDirection(direction);

            // Play sound if exists.
            if (_data.SoundEmitter != null)
                _data.SoundEmitter.Play();
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

        [Header("Optional")]
        [SerializeField]
        private StudioEventEmitter _soundEmitter = null;

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

        /// <summary>
        /// Bullet shot sound effect.
        /// </summary>
        public StudioEventEmitter SoundEmitter => _soundEmitter;

        #endregion
    }
}
