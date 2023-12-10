using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle random spawning.
    /// </summary>
    public abstract class BaseSpawner : MonoBehaviour, ISpawner
    {
        #region ISpawner

        public abstract void Spawn();

        #endregion
    }
}
