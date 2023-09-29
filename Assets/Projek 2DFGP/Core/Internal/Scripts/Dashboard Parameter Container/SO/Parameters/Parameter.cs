using UnityEngine;

namespace JT
{
    /// <summary>
    /// Registered parameter data.
    /// </summary>
    [System.Serializable]
    public abstract class Parameter : ScriptableObject
    {
        #region Variables

        [SerializeField]
        private string _paramKey = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Key of parameter.
        /// </summary>
        public string Key => _paramKey;

        #endregion

        #region Main

        public abstract BakedParameter BakeParam();

        #endregion
    }

    /// <summary>
    /// Registered parameter data.
    /// </summary>
    [System.Serializable]
    public abstract class Parameter<T> : Parameter, IBake<BakedParameter<T>>
    {
        #region Variables

        [SerializeField]
        private T _paramValue = default(T);

        #endregion

        #region Properties

        /// <summary>
        /// Value of parameter.
        /// </summary>
        public T Value
        {
            get => _paramValue;
            set => _paramValue = value;
        }

        #endregion

        #region Parameter

        public override BakedParameter BakeParam() => Bake();

        #endregion

        #region IBake

        public abstract BakedParameter<T> Bake();

        #endregion
    }
}

