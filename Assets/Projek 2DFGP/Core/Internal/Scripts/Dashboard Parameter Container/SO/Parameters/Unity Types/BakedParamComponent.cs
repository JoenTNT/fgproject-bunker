using UnityEngine;

namespace JT
{
    /// <summary>
    /// Baked form parameter Component.
    /// </summary>
    public class BakedParamComponent : BakedParameter<Component>
    {
        #region Constructor

        public BakedParamComponent(Component value) : base(value) { }

        #endregion
    }
}
