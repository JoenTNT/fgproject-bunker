using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// To kill the entity.
    /// </summary>
    public sealed class MultipleKillCommand : MonoBehaviour, IKill
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private GameObject[] _killTargets = null;

        [Header("Properties")]
        [SerializeField]
        private bool _destroyOnKill = false;

        #endregion

        #region Properties

        /// <summary>
        /// Target objects to be kill.
        /// </summary>
        public GameObject[] Targets => _killTargets;

        #endregion

        #region IKill

        public void Kill()
        {
            for (int i = 0; i < _killTargets.Length; i++)
            {
                // Ignore null value.
                if (_killTargets[i] == null) continue;

                // Check kill destroy method.
                if (_destroyOnKill)
                {
                    Destroy(_killTargets[i]);
                    continue;
                }

                // Disable instead of destroy.
                _killTargets[i].SetActive(false);
            }
        }

        #endregion
    }
}

