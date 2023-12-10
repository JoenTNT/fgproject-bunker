using JT.GameEvents;
using System.Collections.Generic;
using UnityEngine;

namespace JT.FGP
{
    /// <summary>
    /// Used to handle any updates in gameplay info UI.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UI_GameplayInfoSection : MonoBehaviour
    {
        #region Variables

        [Header("Requirements")]
        [SerializeField]
        private RectTransform _runtimePlayerStatsContainer = null;

        [Header("Game Events")]
        [SerializeField]
        private GameEventStringUnityObject _onAddRuntimePlayerInfo = null;

        [SerializeField]
        private GameEventString _onDeleteRuntimePlayerInfo = null;

        // Runtime variable data.
        private Dictionary<string, UI_RuntimePlayerStats> _multiplayerStats = new();
        private RectTransform _rectTransform = null;

        #endregion

        #region Properties

        public RectTransform rectTransform => _rectTransform;

        #endregion

        #region Mono

        private void Awake()
        {
            // Init rect transform.
            _rectTransform = (RectTransform)transform;
            if (_multiplayerStats == null) _multiplayerStats = new();

            // Subscribe events.
            _onAddRuntimePlayerInfo.AddListener(ListenOnAddRuntimePlayerInfo);
            _onDeleteRuntimePlayerInfo.AddListener(ListenOnDeleteRuntimePlayerInfo);
        }

        private void OnDestroy()
        {
            // Unsubscribe events.
            _onAddRuntimePlayerInfo.RemoveListener(ListenOnAddRuntimePlayerInfo);
            _onDeleteRuntimePlayerInfo.RemoveListener(ListenOnDeleteRuntimePlayerInfo);
        }

        #endregion

        #region Main

        private void ListenOnDeleteRuntimePlayerInfo(string entityID)
        {
            // Validate existence, abort if not.
            if (!_multiplayerStats.ContainsKey(entityID)) return;

            // Remove the target stats.
            _multiplayerStats.Remove(entityID);
            // TODO: notify replaced UI stats to player.
        }

        private void ListenOnAddRuntimePlayerInfo(string entityID, Object runtimePlayerStats)
        {
            // Validate type object, abort if not.
            if (runtimePlayerStats is not UI_RuntimePlayerStats) return;

            // TODO: notify replaced UI stats to player.
            //if (_multiplayerStats.ContainsKey(entityID)) { }

            // Add new stats to UI.
            var newStats = (UI_RuntimePlayerStats)runtimePlayerStats;
            _multiplayerStats[entityID] = newStats;
            newStats.transform.SetParent(_runtimePlayerStatsContainer);

            // Normalize position and size.
            RectTransform rect = newStats.rectTransform;
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(0f, rect.sizeDelta.y);
        }

        #endregion
    }
}
