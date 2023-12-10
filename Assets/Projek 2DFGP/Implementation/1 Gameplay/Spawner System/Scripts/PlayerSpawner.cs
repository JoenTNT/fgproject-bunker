using JT.GameEvents;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Handle player spawner.
    /// </summary>
    public sealed class PlayerSpawner : MonoBehaviour, ISpawner<PlayerEntity>
    {
        #region Variables

        private static PlayerSpawner s_instance = null;
        private static PlayerEntity s_mainPlayer = null;

        [Header("Requirements")]
        [SerializeField]
        private PlayerEntity _playerPrefab = null;

        [SerializeField]
        private Transform[] _spawnPoints = null;

        [Header("Properties")]
        [SerializeField]
        private bool _spawnMainPlayerOnAwake = true;

        [Header("Game Events")]
        [SerializeField]
        private GameEventStringUnityObject _onMainPlayerKilled = null;

        // Runtime variable data.
        private Transform _tempPoint = null;

        #endregion

        #region Properties

        /// <summary>
        /// The main player reference.
        /// </summary>
        public static PlayerEntity MainPlayer => s_mainPlayer;

        #endregion

        #region Mono

        private void Awake()
        {
            // Destroy extra objects.
            if (s_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            // Set singleton instance.
            s_instance = this;

            // Spawn main player.
            if (_spawnMainPlayerOnAwake) s_mainPlayer = Spawn();

            // Subscribe events.
            _onMainPlayerKilled.AddListener(ListenOnMainPlayerKilled);
        }

        private void OnDestroy()
        {
            // Ignore non singleton instance object.
            if (s_instance != this) return;
            s_instance = null;

            // Release cache of main player.
            s_mainPlayer = null;

            // Unsubscribe events.
            _onMainPlayerKilled.RemoveListener(ListenOnMainPlayerKilled);
        }

        #endregion

        #region ISpawner

        public PlayerEntity Spawn()
        {
            int spawnPoint = Random.Range(0, _spawnPoints.Length);
            _tempPoint = _spawnPoints[spawnPoint];
            return Instantiate(_playerPrefab, _tempPoint.position, _tempPoint.rotation);
        }

        #endregion

        #region Main

        private void ListenOnMainPlayerKilled(string id, Object obj)
        {
            // Validate information.
            if (s_mainPlayer.EntityID != id) return;
            if (obj.GetInstanceID() != s_mainPlayer.GetInstanceID()) return;

            // TODO: Game Over.
        }

        public void Respawn()
        {
            // Main Player must be present.
            if (s_mainPlayer == null)
                s_mainPlayer = Spawn();

            // Reactivate main player.
            // TODO: Respawn main player.
        }

        #endregion
    }
}