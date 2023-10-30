using System;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle runtime reload information object.
    /// </summary>
    [Serializable]
    public sealed class RuntimeReloadInfo : IReload, ICloneable, IInjectDependency<UI_ReloadTimeInfo>
    {
        #region Variables

        [Header("Properties")]
        [SerializeField, Min(0f)]
        private float _initialSecond = 3f;

        // Runtime variable data.
        private UI_ReloadTimeInfo _reloadTimeInfo = null;
        private float _currentSecond = 0f;

        #endregion

        #region IReload

        public float InitialReloadSecond
        {
            get => _initialSecond;
            internal set => _initialSecond = value;
        }

        public float CurrentReloadSecond
        {
            get => _currentSecond;
            internal set
            {
                _currentSecond = value;
                if (_reloadTimeInfo != null)
                    _reloadTimeInfo.SetInfo(_currentSecond);
            }
        }

        public bool IsReloading => _currentSecond > 0f;

        public void OnReloading()
        {
            if (_currentSecond <= 0f) return;
            CurrentReloadSecond -= Time.deltaTime;
        }

        public void ResetReloadTime() => CurrentReloadSecond = _initialSecond;

        public void SkipReload() => CurrentReloadSecond = 0f;

        #endregion

        #region ICloneable

        public object Clone()
        {
            var cloned = new RuntimeReloadInfo();
            cloned._initialSecond = _initialSecond;
            return cloned;
        }

        #endregion

        #region Main

        public void Inject(UI_ReloadTimeInfo instance = null)
        {
            _reloadTimeInfo = instance;
            if (_reloadTimeInfo != null)
                _reloadTimeInfo.SetInfo(_currentSecond);
        }

        #endregion
    }
}
