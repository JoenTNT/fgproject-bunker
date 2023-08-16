using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Object pool returner uniform targeting Unity Object only.
    /// </summary>
    /// <typeparam name="T">Unity object only</typeparam>
    public interface IObjectPoolReturn<T> where T : Object
    {
        /// <summary>
        /// Returning object back to the pool.
        /// </summary>
        /// <param name="obj">Old object</param>
        /// <param name="index">Index in pool</param>
        void ReturnObject(T obj, int? index = null);
    }
}
