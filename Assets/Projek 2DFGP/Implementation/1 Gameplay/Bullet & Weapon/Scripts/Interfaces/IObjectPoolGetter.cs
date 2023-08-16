using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Object pool getter uniform targeting Unity Object only.
    /// </summary>
    /// <typeparam name="T">Unity object only</typeparam>
    public interface IObjectPoolGetter<T> where T : Object
    {
        /// <summary>
        /// Release object from pool.
        /// </summary>
        /// <returns>Object from pool released, may be null if pool is empty</returns>
        T GetObject();
    }
}
