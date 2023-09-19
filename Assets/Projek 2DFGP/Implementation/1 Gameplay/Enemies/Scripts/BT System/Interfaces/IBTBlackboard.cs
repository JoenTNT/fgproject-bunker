namespace JT
{
    /// <summary>
    /// Data container for behaviour tree AI.
    /// </summary>
    public interface IBTBlackboard
    {
        /// <summary>
        /// Getting references data from blackboard.
        /// </summary>
        /// <typeparam name="T">Type of component data reference you want to get.</typeparam>
        /// <param name="paramKey">Key of param key.</param>
        /// <returns>Reference object</returns>
        T GetReference<T>(string paramKey) where T : UnityEngine.Object;
    }
}
