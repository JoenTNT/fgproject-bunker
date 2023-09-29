using System.Collections.Generic;
using UnityEngine;

namespace JT
{
    /// <summary>
    /// Converted dashboard into baked form.
    /// </summary>
    public sealed class BakedDashboard
    {
        #region Variables

        // Runtime variable data.
        private GameObject _target = null;
        private Dictionary<string, BakedParameter> _parameters = null;

        #endregion

        #region Properties

        /// <summary>
        /// Target object owned.
        /// </summary>
        public GameObject Target => _target;

        /// <summary>
        /// All parameters in one dashboard.
        /// </summary>
        public IReadOnlyDictionary<string, BakedParameter> Parameters => _parameters;

        #endregion

        #region Constructor

        private BakedDashboard() { }

        public BakedDashboard(GameObject target, Dictionary<string, BakedParameter> parameters)
        {
            _target = target;
            _parameters = parameters;
        }

        #endregion

        #region Main

        /// <summary>
        /// Bake target object before cherry picking.
        /// </summary>
        /// <param name="bakeTo">Target baked object</param>
        public void BakeObject(in IBakedObject bakeTo) => bakeTo.Bake(_parameters);

        /// <summary>
        /// Assign value to one of the parameter.
        /// </summary>
        /// <typeparam name="T">Specific type of parameter value data</typeparam>
        /// <param name="key">Parameter key name</param>
        /// <param name="value">Parameter value</param>
        public void AssignValue<T>(string key, T value)
        {
            ((BakedParameter<T>)_parameters[key]).Value = value;
        }

        /// <summary>
        /// Get value from one of the parameter.
        /// </summary>
        /// <typeparam name="T">Specific type of parameter value data</typeparam>
        /// <param name="key">Parameter key name</param>
        /// <returns>Parameter value</returns>
        public T GetValue<T>(string key)
        {
            return ((BakedParameter<T>)_parameters[key]).Value;
        }

        #endregion
    }
}
