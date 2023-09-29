using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace JT
{
    /// <summary>
    /// Debugger window fir dashboard parameter container.
    /// </summary>
    public class DPCDebuggerWindow : EditorWindow
    {
        #region Variables

        // Constants.
        private const string CONTAINER_INSTANCE_PROP_KEY = "Instance";
        private const string DICTIONARY_DASHBOARDS_KEY = "_dashboards";

        // Singleton instance.
        private static DPCDebuggerWindow _instance = null;

        // Editor Runtime variable data.
        private Type _containerType = null;
        private PropertyInfo _containerInstanceProp = null;
        private GlobalDPContainer _containerInstance = null;
        private FieldInfo _dictionaryContainerField = null;
        private Dictionary<int, BakedDashboard> _dictionaryRef = null;

        #endregion

        #region Properties

        /// <summary>
        /// Dashboard list reference.
        /// </summary>
        private Dictionary<int, BakedDashboard> DashboardRefs
        {
            get
            {
                if (_dictionaryRef != null) return _dictionaryRef;
                _containerType = typeof(GlobalDPContainer);
                _containerInstanceProp = _containerType.GetProperty(CONTAINER_INSTANCE_PROP_KEY,
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                _containerInstance = (GlobalDPContainer)_containerInstanceProp.GetValue(null, null);
                _dictionaryContainerField = _containerType.GetField(DICTIONARY_DASHBOARDS_KEY,
                    BindingFlags.Instance | BindingFlags.NonPublic);
                _dictionaryRef = (Dictionary<int, BakedDashboard>)_dictionaryContainerField
                    .GetValue(_containerInstance);
                return _dictionaryRef;
            }
        }

        #endregion

        #region Editor

        private void OnDestroy()
        {
            // Check not the window instance, then ignore.
            if (this != _instance) return;

            // Release instance from memory.
            _instance = null;
        }

        private void OnEnable()
        {
            // Initialize reflections of instance.
            Debug.Log(DashboardRefs);
        }

        private void OnGUI()
        {
            // TODO: Draw dictionary to check each dashboard informations.
            //GUI.BeginScrollView();
            //GUI.EndScrollView();
        }

        #endregion

        #region Main

        private void DrawHeaderTabs()
        {
            // TODO: Draw header tabs editor.
        }

        #endregion

        #region Statics

        [MenuItem("Framework/JT/DPC/Debugger")]
        public static void OpenDebuggerWindow()
        {
            // Make sure there's only one debugger in plugin.
            if (_instance != null)
            {
                _instance.Show();
                return;
            }

            // Create debugger window on first time.
            _instance = GetWindow<DPCDebuggerWindow>("DPC Debugger");
            _instance.position = new Rect(100f, 100f, 480f, 400f);
            _instance.Show();
        }

        #endregion
    }
}
