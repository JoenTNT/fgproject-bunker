using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// FSM parameter string type.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New String Param",
        menuName = "JT Framework/FSM/Param/String")]
    public sealed class FSM_StringParam : FSM_ParameterData, IParameterData<string>
    {
        #region Variables

        [SerializeField]
        private string _paramValue = string.Empty;

        #endregion

        #region IParameterData

        public string ParamValue
        {
            get => _paramValue;
            set => _paramValue = value;
        }

        #endregion
    }
}
