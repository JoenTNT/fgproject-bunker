using UnityEditor;
using UnityEngine;

namespace JT
{
    /// <summary>
    /// Customize draw global DP container on inspector.
    /// </summary>
    [CustomEditor(typeof(GlobalDPContainer))]
    public sealed class GlobalDPContainer_Editor : Editor
    {
        #region Variables

        // Editor Runtime variable data.
        private GUIContent _openDebuggerContent = new GUIContent("Open Debugger");

        #endregion

        #region Editor

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.HelpBox("All dashboards will be send to this container. " +
                "You can open debugger in runtime.", MessageType.Info);
            if (GUILayout.Button(_openDebuggerContent, GUI.skin.button))
                DPCDebuggerWindow.OpenDebuggerWindow();
            EditorGUILayout.EndVertical();
        }

        #endregion
    }
}
