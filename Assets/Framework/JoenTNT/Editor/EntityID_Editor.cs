using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace JT.Editors
{
    /// <summary>
    /// Entity ID class editor.
    /// </summary>
    [CustomEditor(typeof(EntityID))]
    internal class EntityID_Editor : Editor
    {
        #region Variable

        private const int ID_MODE_INSTANCE = 1;
        private const int ID_MODE_MANUAL = 2;

        private EntityID _entityID = null;
        //private SerializedProperty _idModeProp = null;
        private SerializedProperty _idProp = null;

        private FieldInfo _idModeField = null;
        private FieldInfo _isInitField = null;

        //private GUIStyle _textRightAlignStyle = null;

        private Type _enumMode = null;
        private string[] _enumModeNames = null;
        private int _modeValue = 0;
        private bool _foldoutDebug = false;

        #endregion

        #region Editor

        private void OnEnable()
        {
            _entityID = (EntityID)target;
            //_idModeProp = serializedObject.FindProperty("_mode");
            _idProp = serializedObject.FindProperty("_id");

            _enumMode = _entityID.GetType().GetNestedType("Mode", BindingFlags.NonPublic);
            _enumModeNames = _enumMode.GetEnumNames();
            _idModeField = _entityID.GetType().GetField("_mode", BindingFlags.Instance | BindingFlags.NonPublic);
            _isInitField = _entityID.GetType().GetField("_isInit", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Serialized Section
            EditorGUILayout.BeginVertical(GUI.skin.box);
            _modeValue = (int)_idModeField.GetValue(_entityID);
            if (!Application.isPlaying)
            {
                EditorGUILayout.BeginHorizontal();

                //EditorGUILayout.LabelField("ID Mode", GUILayout.MaxWidth(80f));

                _modeValue = EditorGUILayout.Popup(_modeValue, _enumModeNames);
                _idModeField.SetValue(_entityID, _modeValue);
                //_idModeProp.serializedObject.ApplyModifiedProperties();

                //_textRightAlignStyle = GUI.skin.label;
                //_textRightAlignStyle.alignment = TextAnchor.MiddleRight;
                //EditorGUILayout.LabelField($"Value: {(int)Enum.Parse(_enumMode, _enumModeNames[_modeValue])}",
                //    _textRightAlignStyle, GUILayout.MaxWidth(80f));

                EditorGUILayout.EndHorizontal();
            }
            /* New Line */
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("ID =", GUILayout.MaxWidth(24f));
            if ((_modeValue == ID_MODE_MANUAL || Application.isPlaying) && _modeValue != ID_MODE_INSTANCE)
            {
                _idProp.stringValue = EditorGUILayout.TextField(_idProp.stringValue);
            }
            else
            {
                GUI.enabled = false;
                EditorGUILayout.TextField(_entityID.ID);
                GUI.enabled = true;
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            /* Spacing */ EditorGUILayout.Space(2); /* Spacing */

            // Debugger Section
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel++;
            _foldoutDebug = EditorGUILayout.Foldout(_foldoutDebug, "Debug");
            if (_foldoutDebug)
            {
                EditorGUI.indentLevel++;
                GUI.enabled = false;
                EditorGUILayout.Toggle("Is Initialized", (bool)_isInitField.GetValue(_entityID));
                GUI.enabled = true;
                EditorGUI.indentLevel--;
            }
            if (!Application.isPlaying)
                _isInitField.SetValue(_entityID, false);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();

            if (GUI.changed) SaveSerializeField();

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region Main

        private void SaveSerializeField()
        {
            EditorUtility.SetDirty(_entityID);
        }

        #endregion
    }
}