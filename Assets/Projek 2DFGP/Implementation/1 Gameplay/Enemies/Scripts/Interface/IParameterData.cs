namespace JT.FGP
{
    /// <summary>
    /// Single parameter data interface.
    /// </summary>
    public interface IParameterData
    {
        /// <summary>
        /// Key of this parameter.
        /// </summary>
        string ParamKey { get; }
    }

    /// <summary>
    /// Single parameter data interface with specific type.
    /// </summary>
    /// <typeparam name="T">Parameter data type</typeparam>
    public interface IParameterData<T> : IParameterData
    {
        T ParamValue { set; get; }
    }
}

