using System.Collections.Generic;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Container that contains registered parameters.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Parameter Container",
        menuName = "JT Framework/FSM/Parameter Container")]
    public sealed class FSMSO_ParameterContainer : ScriptableObject, IParameterContainerData,
        IDuplicateSO<FSMSO_ParameterContainer>
    {
        #region Variables

        [SerializeField]
        private FSMSO_ParameterData[] _parameterData = new FSMSO_ParameterData[0];

        // Runtime variable data.
        private Dictionary<string, IParameterData> _dictParamData = new();

        #endregion

        #region IParameterContainerData

        public int CountKeys => _parameterData.Length;

        public IParameterData<T> GetParamValue<T>(string key)
        {
            // Always check init on the first run.
            CheckInit();

            // Get value of parameter.
            return _dictParamData.ContainsKey(key) ? (IParameterData<T>)_dictParamData[key] : null;
        }

        public bool KeyExists(string key)
        {
            // Always check init on the first run.
            CheckInit();

            // Is exists.
            return _dictParamData.ContainsKey(key);
        }

        public void SetParamValue<T>(string key, T value)
        {
            // Always check init on the first run.
            CheckInit();

            // Check type validation.
            if (_dictParamData[key] is not IParameterData<T>) return;

            // Set parameter value.
            var param = (IParameterData<T>)_dictParamData[key];
            param.ParamValue = value;
        }

        #endregion

        #region IDuplicateSO

        public FSMSO_ParameterContainer Duplicate()
        {
            // TODO: Implement duplicate.
            throw new System.NotImplementedException();
        }

        #endregion

        #region Main

        private void CheckInit()
        {
            // Check already exists or filled, then abort this process.
            if (_dictParamData != null && _dictParamData.Count > 0)
                return;

            // Generate dictionary.
            if (_dictParamData == null) _dictParamData = new();
            for (int i = 0; i < _parameterData.Length; i++)
            {
                if (string.IsNullOrEmpty(_parameterData[i].ParamKey))
                    continue;
                _dictParamData[_parameterData[i].ParamKey] = _parameterData[i];
            }
        }

        #endregion
    }
}
