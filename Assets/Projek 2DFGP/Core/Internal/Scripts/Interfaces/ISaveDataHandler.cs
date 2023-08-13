using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle saving data into database.
    /// </summary>
    public interface ISaveDataHandler
    {
        /// <summary>
        /// Saving data method.
        /// </summary>
        void SaveData();
    }

    /// <summary>
    /// Handle saving data into database.
    /// </summary>
    public interface IDatabaseSaveHandler<T>
    {
        /// <summary>
        /// Saving target data method.
        /// </summary>
        /// <param name="data">Target data</param>
        void SaveData(T data);
    }
}
