using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle ranged weapon reloading.
    /// </summary>
    [System.Serializable]
    public struct RW_Reloader : IReload
    {
        #region Variables

        [Header("Properties")]
        [SerializeField, Min(0f)]
        private float _initialReloadSecond;

        // Runtime variable data.
        private float _currentReloadSecond;

        #endregion

        #region IReload

        public float InitialReloadSecond => _initialReloadSecond;

        public float CurrentReloadSecond => _currentReloadSecond;

        public bool IsReloading => _currentReloadSecond > 0f;

        public void OnReloading() => _currentReloadSecond -= Time.deltaTime;

        public void ResetReloadTime() => _currentReloadSecond = _initialReloadSecond;

        public void SkipReload() => _currentReloadSecond = 0f;

        #endregion
    }
}

