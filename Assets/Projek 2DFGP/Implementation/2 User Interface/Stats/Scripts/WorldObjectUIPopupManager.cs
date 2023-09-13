using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Manage UI object popup in world space.
    /// </summary>
    public sealed class WorldObjectUIPopupManager : MonoBehaviour
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private EntityID _id = null;

        [Header("Properties")]
        [SerializeField]
        private string _damageCloudPoolKey = string.Empty;

        [Header("Optional")]
        [SerializeField]
        private GameObjectPool _damageCloudPool = null;

        [Header("Game Events")]
        [SerializeField]
        private GameEventTwoString _requestDamageCloudPoolCallback = null;

        [SerializeField]
        private GameEventStringUnityObject _assignDamageCloudPoolCallback = null;

        [SerializeField]
        private GameEventVector2Float _onEntityHitOnTheSpot = null;

        // Runtime variable data.
        private UI_ThrowHitDamageCloudValue _tempCloud = null;
        private GameObject _tempObjUI = null;

        #endregion

        #region Mono

        private void Awake()
        {
            // Subscribe events
            _assignDamageCloudPoolCallback.AddListener(ListenAssignDamageCloudPoolCallback);
            _onEntityHitOnTheSpot.AddListener(ListenOnEntityHitOnTheSpot);
        }

        private void OnDestroy()
        {
            // Subscribe events
            _assignDamageCloudPoolCallback.RemoveListener(ListenAssignDamageCloudPoolCallback);
            _onEntityHitOnTheSpot.RemoveListener(ListenOnEntityHitOnTheSpot);
        }

        private void Start()
        {
            // Request for pool of objects.
            _requestDamageCloudPoolCallback.Invoke(_id.ID, _damageCloudPoolKey);
        }

        #endregion

        #region Main

        private void ListenAssignDamageCloudPoolCallback(string entityID, Object pool)
        {
            // Check validation.
            if (_id.ID != entityID) return;
            if (pool is not GameObjectPool) return;

            // Assign pool.
            _damageCloudPool = (GameObjectPool)pool;
        }

        private void ListenOnEntityHitOnTheSpot(Vector2 spotPosition, float damageHit)
        {
            // Get cloud object.
            _tempObjUI = _damageCloudPool.GetObject();

            // Set informations and run the cloud damage.
            if (!_tempObjUI.TryGetComponent(out _tempCloud)) return;
            _tempCloud.SetValue(damageHit);
            _tempCloud.Run(spotPosition);
        }

        #endregion
    }
}

