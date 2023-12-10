using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Used to drop item physically in game.
    /// </summary>
    public class DropPhysicalItemFunc : MonoBehaviour, IDropObject<Transform>
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private Transform _dropPoint = null;

        [Header("Properties")]
        [SerializeField, Min(0f)]
        private float _radiusDrop = 1f;

        // Runtime variable data.
        private Vector2 _dropPos;

        #endregion
#if UNITY_EDITOR
        #region Mono

        private void OnDrawGizmosSelected()
        {
            if (_dropPoint == null) return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_dropPoint.position, _radiusDrop);
            Gizmos.color = Color.white;
        }

        #endregion
#endif
        #region IDropObject

        public void DropObject(Transform obj)
        {
            // Send to randomized drop position.
            _dropPos = _dropPoint.position;
            _dropPos += Random.insideUnitCircle * _radiusDrop;
            obj.position = _dropPos;
        }

        #endregion
    }
}
