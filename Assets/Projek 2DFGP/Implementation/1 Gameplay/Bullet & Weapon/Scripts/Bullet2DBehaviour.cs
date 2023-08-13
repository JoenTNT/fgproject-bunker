using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Generic bullet behaviour for 2D game.
    /// </summary>
    public abstract class Bullet2DBehaviour : MonoBehaviour
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private Topview2DMovementFunc _moveFunc = null;

        [SerializeField]
        private Topview2DLookAtFunc _lookFunc = null;

        #endregion

        #region Properties

        /// <summary>
        /// Function to move the bullet.
        /// </summary>
        protected Topview2DMovementFunc MoveFunc => _moveFunc;

        /// <summary>
        /// Function to rotate bullet by shoot direction.
        /// </summary>
        protected Topview2DLookAtFunc LookFunc => _lookFunc;

        #endregion
    }
}
