using UnityEngine;

namespace JT
{
    /// <summary>
    /// Baked parameter base class.
    /// </summary>
    public abstract class BakedParameter
    {
        #region Statics

        /// <summary>
        /// Convert baked parameter straight to component.
        /// </summary>
        public static implicit operator Component(BakedParameter param)
        {
            if (param is BakedParamComponent)
                return (param as BakedParamComponent).Value;
            return null;
        }

        #endregion
    }

    /// <summary>
    /// Baked type parameter.
    /// </summary>
    /// <typeparam name="T">Any type</typeparam>
    public abstract class BakedParameter<T> : BakedParameter
    {
        #region Variables

        // Runtime variable data.
        private T _paramValue = default(T);

        #endregion

        #region Properties

        public virtual T Value
        {
            get => _paramValue;
            set => _paramValue = value;
        }

        #endregion

        #region Constructor

        public BakedParameter(T value) => _paramValue = value;

        #endregion
    }
}
