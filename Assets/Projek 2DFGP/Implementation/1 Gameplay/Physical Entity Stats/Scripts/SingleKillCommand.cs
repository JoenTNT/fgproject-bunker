using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// To kill the entity.
    /// </summary>
    public sealed class SingleKillCommand : MonoBehaviour, IKill
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private GameObject _targetKill = null;

        [Header("Properties")]
        [SerializeField]
        private bool _destroyOnKill = false;

        #endregion

        #region Properties

        /// <summary>
        /// Target object to be kill.
        /// </summary>
        public GameObject Target => _targetKill;

        #endregion

        #region IKill

        public void Kill()
        {
            // Check kill destroy method.
            if (_destroyOnKill)
            {
                Destroy(_targetKill);
                return;
            }

            // Disable instead of destroy.
            _targetKill.SetActive(false);
        }

        #endregion
    }
}

