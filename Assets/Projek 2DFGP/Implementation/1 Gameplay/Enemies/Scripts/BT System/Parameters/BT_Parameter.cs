using UnityEngine;

namespace JT
{
    /// <summary>
    /// Registered parameter data.
    /// </summary>
    public abstract class BT_Parameter<T> : Object
    {
        #region Variables

        // Runtime variable data.
        private string _paramKey = string.Empty;
        private T _paramValue = default(T);

        #endregion

        #region Properties

        /// <summary>
        /// Key of parameter.
        /// </summary>
        public string Key
        {
            get => _paramKey;
            set => _paramKey = value; // TODO: Encapsulate setter.
        }

        /// <summary>
        /// Value of parameter.
        /// </summary>
        public T Value
        {
            get => _paramValue;
            set => _paramValue = value;
        }

        #endregion
    }
}

