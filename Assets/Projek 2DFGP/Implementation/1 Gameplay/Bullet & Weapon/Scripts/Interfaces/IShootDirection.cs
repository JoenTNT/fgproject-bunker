using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Shoot direction informations.
    /// </summary>
    /// <typeparam name="T">Direction data type</typeparam>
    public interface IShootDirection<T> where T : notnull
    {
        /// <summary>
        /// Shoot direction info.
        /// </summary>
        T ShootDirection { get; }
    }
}

