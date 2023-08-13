using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle loading data from database.
    /// </summary>
    public interface ILoadDataHandler
    {
        /// <summary>
        /// Loading data method.
        /// </summary>
        void LoadData();
    }

    /// <summary>
    /// Handle loading data from database.
    /// </summary>
    public interface IDatabaseLoadHandler<T>
    {
        /// <summary>
        /// Loading type data method.
        /// </summary>
        /// <returns>Loaded data</returns>
        T LoadData();
    }
}
