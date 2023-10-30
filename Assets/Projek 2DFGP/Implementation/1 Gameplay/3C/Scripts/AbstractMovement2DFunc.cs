using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// An abstract movement behaviour for 2D game.
    /// </summary>
    public abstract class AbstractMovement2DFunc : MonoBehaviour, IHandleMovement2D
    {
        #region Variables

        [Header("Properties")]
        [SerializeField]
        private float _speedMultiplier = 1.0f;

        #endregion

        #region Properties

        /// <summary>
        /// Current calculated movement velocity.
        /// </summary>
        public abstract Vector2 Velocity { get; }

        /// <summary>
        /// Movement speed multiplier.
        /// </summary>
        public float SpeedMultiplier
        {
            get => _speedMultiplier;
            set => _speedMultiplier = value;
        }

        #endregion

        #region IHandleMovement2D

        public abstract void OnMove();

        #endregion
    }
}

