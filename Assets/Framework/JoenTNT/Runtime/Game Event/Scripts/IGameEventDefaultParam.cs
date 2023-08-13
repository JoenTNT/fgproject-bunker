namespace JT.GameEvents
{
    /// <summary>
    /// Game event one parameter default values.
    /// </summary>
    internal interface IGameEventDefaultParam<T>
    {
        /// <summary>
        /// Default first parameter.
        /// </summary>
        T DefaultParam1 { get; }
    }

    /// <summary>
    /// Game event two parameter default values.
    /// </summary>
    internal interface IGameEventDefaultParam<T1, T2> : IGameEventDefaultParam<T1>
    {
        /// <summary>
        /// Default second parameter.
        /// </summary>
        T2 DefaultParam2 { get; }
    }

    /// <summary>
    /// Game event three parameter default values.
    /// </summary>
    internal interface IGameEventDefaultParam<T1, T2, T3> : IGameEventDefaultParam<T1, T2>
    {
        /// <summary>
        /// Default third parameter.
        /// </summary>
        T3 DefaultParam3 { get; }
    }

    /// <summary>
    /// Game event four parameter default values.
    /// </summary>
    internal interface IGameEventDefaultParam<T1, T2, T3, T4> : IGameEventDefaultParam<T1, T2, T3>
    {
        /// <summary>
        /// Default fourth parameter.
        /// </summary>
        T4 DefaultParam4 { get; }
    }
}