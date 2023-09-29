using UnityEngine;

namespace JT
{
    /// <summary>
    /// Baked form parameter Collider2D.
    /// </summary>
    public class BakedParamCollider2D : BakedParameter<Collider2D>
    {
        #region Constructor

        public BakedParamCollider2D(Collider2D value) : base(value) { }

        #endregion
    }
}
