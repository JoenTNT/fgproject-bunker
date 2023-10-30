namespace JT.FGP
{
    /// <summary>
    /// Injects dependency to entity.
    /// </summary>
    /// <typeparam name="T">Object dependency type reference</typeparam>
    public interface IInjectDependency<T> where T : class
    {
        /// <summary>
        /// Inject dependency.
        /// </summary>
        /// <param name="instance">Target dependency</param>
        void Inject(T instance);
    }
}
