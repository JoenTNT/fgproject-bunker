using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Parameter data base.
    /// </summary>
    public abstract class FSMSO_ParameterData : ScriptableObject, IParameterData
    {
        #region Variables

        [SerializeField]
        private string _paramKey = string.Empty;

        #endregion

        #region IParameterData

        public string ParamKey => _paramKey;

        #endregion

        #region Main

        /// <summary>
        /// Change with specific type of parameter value.
        /// WARNING: Must be correct type and errors cannot be handled.
        /// </summary>
        /// <typeparam name="T">Parameter data type</typeparam>
        /// <returns>Parameter data</returns>
        public T AsType<T>() where T : IParameterData
        {
            if (this is T)
                return (T)(object)this;
            return default(T);
        }

        #endregion
    }
}

