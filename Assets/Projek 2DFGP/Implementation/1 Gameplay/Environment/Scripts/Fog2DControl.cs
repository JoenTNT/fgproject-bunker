using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle fog control in game.
    /// </summary>
    public sealed class Fog2DControl : MonoBehaviour
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private SpriteRenderer _targetFog = null;

        [Header("Properties")]
        [SerializeField]
        private bool _copyMaterial = false;

        // Runtime variable data.
        private Material _materialRef = null;
        private Material _copiedMaterial = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Assign material reference.
            _materialRef = _targetFog.sharedMaterial;
        }

        private void Start()
        {
            // Check if material need to be copied.
            if (_copyMaterial)
            {
                // Copy the material from renderer.
                _copiedMaterial = new Material(_materialRef);

                // Assign to target renderer.
                _targetFog.material = _copiedMaterial;
            }
        }

        private void OnDestroy()
        {
            Destroy(_copiedMaterial);
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            // Validate shader only in editor.
            if (Application.isPlaying) return;
            if (_targetFog == null) return;

            // Deny if material is not using a fog shader.
            if (!_targetFog.sharedMaterial.shader.name.ToLower().Contains("fog"))
            {
                _targetFog = null;
                Debug.LogWarning("[DEBUG] Must use material with fog shader.");
            }
        }
#endif
        #endregion
    }
}
