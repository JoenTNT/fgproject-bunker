#if FMOD
using FMODUnity;
#endif
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Bullet 2D object control.
    /// </summary>
    [RequireComponent(typeof(DestroyTimer))]
    public class Bullet2DControl : MonoBehaviour, IShootDirectionCommand<Vector2>, IRequiredReset,
        IOwnerID<string>
    {
        #region Variables

        [SerializeField]
        private Bullet2DControlData _data = new Bullet2DControlData();

        // Runtime variable data.
        private AbstractMovement2DFunc _moveFunc = null;
        private DestroyTimer _destroyTimer = null;
        private string _shooterID = string.Empty;
        private bool _isShoot = false;

        #endregion

        #region Properties

        /// <summary>
        /// Is sprite enabled?
        /// </summary>
        public bool SpriteEnabled
        {
            get => _data.Sprite.enabled;
            set => _data.Sprite.enabled = value;
        }

        #endregion

        #region Mono

        private void Update()
        {
            // Check if bullet has been shoot.
            if (!_isShoot) return;

            // Detect any target hit.
            _data.Hitter.OnDetectHit(transform.position, _data.MoveFunc.Velocity * Time.deltaTime);
        }

        private void LateUpdate()
        {
            // Move a bullet until all target hit.
            if (!_data.Hitter.IsDone)
                _data.MoveFunc.OnMove();
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
            _moveFunc = _data.MoveFunc;
            _moveFunc.SpeedMultiplier = speed;
            if (_moveFunc is IDirectionBaseMovement2D)
                ((IDirectionBaseMovement2D)_moveFunc).SetMoveDirection(direction.normalized);
            _isShoot = true;

            // Bullet facing forward of the aim direction.
            _data.RotateFunc.SetInstantLookDirection(direction);
#if FMOD
            // Play sound if exists.
            if (_data.SoundEmitter != null && !_data.DontPlaySoundOnShot)
                _data.SoundEmitter.Play();
#endif
            // Detect hit on first shot.
            _data.Hitter.OnDetectHit(transform.position, _data.MoveFunc.Velocity * Time.deltaTime);
        }

        #endregion

        #region IOwnerID

        /// <summary>
        /// The shooter ID.
        /// </summary>
        public string OwnerID
        {
            get => _shooterID;
            set => _shooterID = value;
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
        private AbstractMovement2DFunc _moveFunc = null;

        [SerializeField]
        private InstantRotation2DFunc _rotateFunc = null;

        [SerializeField]
        private Bullet2DHitter _hitter = null;

        [SerializeField]
        private SpriteRenderer _bulletSprite = null;

        [Header("Properties")]
        [SerializeField]
        private bool _dontPlaySoundOnShot = false;
#if FMOD
        [Header("Optional")]
        [SerializeField]
        private StudioEventEmitter _soundEmitter = null;
#endif
        #endregion

        #region Properties

        /// <summary>
        /// Function to move the bullet.
        /// </summary>
        public AbstractMovement2DFunc MoveFunc => _moveFunc;

        /// <summary>
        /// Function to rotate bullet by shoot direction.
        /// </summary>
        public InstantRotation2DFunc RotateFunc => _rotateFunc;

        /// <summary>
        /// Hitter handler.
        /// </summary>
        public Bullet2DHitter Hitter => _hitter;

        /// <summary>
        /// Bullet shot sound effect.
        /// </summary>
        public StudioEventEmitter SoundEmitter => _soundEmitter;

        /// <summary>
        /// Bullet sprite.
        /// </summary>
        public SpriteRenderer Sprite => _bulletSprite;

        /// <summary>
        /// Don't play sound when shot command called.
        /// </summary>
        public bool DontPlaySoundOnShot => _dontPlaySoundOnShot;

        #endregion
    }
}
