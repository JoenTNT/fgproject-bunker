#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
#endif
using UnityEngine;

namespace JT
{
    /// <summary>
    /// Main container to store all dashboard objects and parameters.
    /// </summary>
    public class GlobalDPContainer : ScriptableObject
    {
        #region Variables
#if UNITY_EDITOR
        private const string RESOURCE_DIRECTORY_PATH = "Assets/Resources/";
#endif
        private static readonly string RESOURCE_FILENAME = $"{nameof(GlobalDPContainer)}.asset";

        private static GlobalDPContainer _instance = null;

        // Runtime variable data.
        private Dictionary<int, BakedDashboard> _dashboards = new();

        #endregion

        #region Properties

        /// <summary>
        /// Global container for baked dashboard and parameter.
        /// </summary>
        internal static GlobalDPContainer Instance
        {
            get
            {
                // Check instance already exists.
                if (_instance != null) return _instance;
#if UNITY_EDITOR
                _instance = Resources.Load<GlobalDPContainer>(nameof(GlobalDPContainer));
                if (_instance == null) CreateGlobalContainer();
                else return _instance;
#endif
                return _instance = Resources.Load<GlobalDPContainer>(nameof(GlobalDPContainer));
            }
        }

        #endregion

        #region Main

        internal void RegisterDashboard(BakedDashboard dashboard)
        {
            // Create dictionary if not yet created.
            if (_dashboards == null) _dashboards = new();

            // Add new baked dashboard.
            _dashboards[dashboard.Target.GetInstanceID()] = dashboard;
        }

        /// <summary>
        /// Get dashboard reference.
        /// </summary>
        /// <param name="instanceID">Game object instance</param>
        /// <returns>Baked dashboard reference</returns>
        public static BakedDashboard GetDashboard(int instanceID) => Instance._dashboards[instanceID];

        /// <summary>
        /// This will release dashboard from container.
        /// Always release before destroying the.
        /// </summary>
        /// <param name="instanceID">Game object instance</param>
        public static void ReleaseDashboard(int instanceID) => Instance._dashboards.Remove(instanceID);

        #endregion
#if UNITY_EDITOR
        #region Statics

        /// <summary>
        /// To create a global container.
        /// </summary>
        [MenuItem("Framework/JT/DPC/Initalize Global Container")]
        private static void CreateGlobalContainer()
        {
            // Check if instance already exists.
            if (Instance != null)
            {
                Debug.LogWarning("[WARNING] Container already exists in resource folder.", _instance);
                return;
            }

            // Create instance, not work if already exists.
            var gdata = CreateInstance<GlobalDPContainer>();

            // Save to resource folder.
            string p = $"{RESOURCE_DIRECTORY_PATH}{RESOURCE_FILENAME}";
            AssetDatabase.CreateAsset(gdata, p);
            _instance = AssetDatabase.LoadAssetAtPath<GlobalDPContainer>(p);

            Selection.activeObject = _instance;
            EditorGUIUtility.PingObject(_instance);
            AssetDatabase.SaveAssets();
        }

        #endregion
#endif
    }
}
