using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Preset information data for entity that require reloading function.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Reload Time Info Preset",
        menuName = "FGP/Presets/Reload Time Information")]
    public sealed class ReloadTimeInfoPreset : ScriptableObject, ICreateRuntimeObject<RuntimeReloadInfo>
    {
        #region Variables

        [SerializeField]
        private RuntimeReloadInfo _presetData = null;

        #endregion

        #region ICreateRuntimeObject

        public RuntimeReloadInfo CreateRuntimeObject() => (RuntimeReloadInfo)_presetData.Clone();

        #endregion
    }
}
