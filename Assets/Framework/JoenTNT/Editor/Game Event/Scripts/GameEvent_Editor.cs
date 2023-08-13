using System;
using System.Collections.Generic;
using System.Reflection;
using JT.Editors.Utilities;
using JT.GameEvents;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace JT.Editors
{
    /// <summary>
    /// Editor for abstract game event.
    /// </summary>
    [CustomEditor(typeof(GameEvent), editorForChildClasses: true)]
    internal class GameEvent_Editor : Editor
    {
        #region struct

        protected class StyleData
        {
            #region Variable

            // Contents
            public readonly GUIContent invokeNoParamEventContent;
            public readonly GUIContent invokeByPlaceholderContent;
            public readonly GUIContent invokeByDefaultContent;
            public readonly GUIContent removeAllListenerContent;

            public readonly GUIContent param1PlaceholderContent;
            public readonly GUIContent param1DefaultContent;
            public readonly GUIContent param2PlaceholderContent;
            public readonly GUIContent param2DefaultContent;
            public readonly GUIContent param3PlaceholderContent;
            public readonly GUIContent param3DefaultContent;
            public readonly GUIContent param4PlaceholderContent;
            public readonly GUIContent param4DefaultContent;

            #endregion

            #region Constructor

            public StyleData()
            {
                invokeNoParamEventContent = new GUIContent("Invoke Event", "Invoke game event.");
                invokeByPlaceholderContent = new GUIContent("Invoke By Placeholder", "Invoke sample game event with placeholder data.");
                invokeByDefaultContent = new GUIContent("Invoke By Default Value", "Invoke game event using default value.");
                removeAllListenerContent = new GUIContent("Remove All Listeners", "Remove All Listeners from this event.");

                param1PlaceholderContent = new GUIContent("Param 1", "First Parameter. (Not included in Build)");
                param1DefaultContent = new GUIContent("Default Value 1", "Default value of first parameter. (Included in Build)");
                param2PlaceholderContent = new GUIContent("Param 2", "Second Parameter. (Not included in Build)");
                param2DefaultContent = new GUIContent("Default Value 2", "Default value of second parameter. (Included in Build)");
                param3PlaceholderContent = new GUIContent("Param 3", "Third Parameter. (Not included in Build)");
                param3DefaultContent = new GUIContent("Default Value 3", "Default value of third parameter. (Included in Build)");
                param4PlaceholderContent = new GUIContent("Param 4", "Fourth Parameter. (Not included in Build)");
                param4DefaultContent = new GUIContent("Default Value 4", "Default value of fourth parameter. (Included in Build)");
            }

            #endregion
        }

        #endregion

        #region Variable

        // Constant variables
        protected const string VARSTR_INVOKELOGGING = "_invokeLogging";
        protected const string VARSTR_LIMITLOGGING = "_limitLoggingCount";
        protected const string VARSTR_LISTOFLOGS = "_logs";
        protected const string VARSTR_LISTOFSUBS = "_subscribedEvents";
        protected const string MTDSTR_INVOKESAMPLE = "InvokeByPlaceholder";

        // Main variables
        protected StyleData sd = null;
        protected GameEvent _gameEvent = null;

        // Info variables
        private FieldInfo _invokeLoggingField = null;
        private FieldInfo _limitLoggingCountField = null;
        private FieldInfo _logsField = null;
        private FieldInfo _subscribedEventsField = null;
        protected MethodInfo _invokeByPlaceholderMethod = null;

        // Utility variables
        private ScrollableItemPopupContent _popupGameEventTypes = null;
        private ReorderableList _drawLogList = null;
        private ReorderableList _drawSubscribedList = null;

        // Other variables
        private List<GameEvent.Logging> _logsValue = null;
        private List<GameEvent.Logging> _subscribedEventsValue = null;
        private List<Type> _allGameEventType = null;
        private List<string> _allGameEventTypeName = null;
        private string[] _arrayedAllGameEventTypeNames = null;
        private Type _changeEventType = null;
        private int _selectedGameEventTypeName = 0;
        private int _limitLoggingCountValue = 0;
        private bool _openDebugFoldout = false;
        private bool _invokeLoggingValue = false;
        private bool _foldoutDrawLogList = false;
        private bool _foldoutDrawSubscribedList = false;

        #endregion

        #region Editor

        protected virtual void OnEnable()
        {
            sd = new StyleData();
            _gameEvent = (GameEvent)target;

            _invokeLoggingField = typeof(GameEvent).GetField(VARSTR_INVOKELOGGING, BindingFlags.Instance | BindingFlags.NonPublic);
            _limitLoggingCountField = typeof(GameEvent).GetField(VARSTR_LIMITLOGGING, BindingFlags.Instance | BindingFlags.NonPublic);
            _logsField = typeof(GameEvent).GetField(VARSTR_LISTOFLOGS, BindingFlags.Instance | BindingFlags.NonPublic);
            _subscribedEventsField = typeof(GameEvent).GetField(VARSTR_LISTOFSUBS, BindingFlags.Instance | BindingFlags.NonPublic);
            _invokeByPlaceholderMethod = typeof(GameEvent).GetMethod(MTDSTR_INVOKESAMPLE, BindingFlags.NonPublic | BindingFlags.Instance);

            if (_allGameEventType == null)
            {
                _allGameEventType = new List<Type>();
                _allGameEventTypeName = new List<string>();
                GetAllGameEventType();
                _arrayedAllGameEventTypeNames = _allGameEventTypeName.ToArray();
            }

            _logsValue = (List<GameEvent.Logging>)_logsField.GetValue(_gameEvent);
            _subscribedEventsValue = (List<GameEvent.Logging>)_subscribedEventsField.GetValue(_gameEvent);
            _drawLogList = new ReorderableList(_logsValue, typeof(GameEvent.Logging), false, true, false, false);
            _drawLogList.drawHeaderCallback = DrawLogListHeader;
            _drawLogList.drawElementCallback = DrawLogListElement;
            _drawLogList.elementHeightCallback = DrawLogListHeightElement;
            _drawLogList.footerHeight = 0f;
            _drawSubscribedList = new ReorderableList(_subscribedEventsValue, typeof(GameEvent.Logging), false, true, false, false);
            _drawSubscribedList.drawHeaderCallback = DrawSubscribedListHeader;
            _drawSubscribedList.drawElementCallback = DrawSubscribedListElement;
            _drawSubscribedList.elementHeightCallback = DrawSubscribedListHeightElement;
            _drawSubscribedList.footerHeight = 0f;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            DrawMainGameEventData();
            /* Spacing */
            EditorGUILayout.Space(4); /* Spacing */
            DrawDefaultGameEventData();
            EditorGUILayout.EndVertical();
            /* Spacing */
            EditorGUILayout.Space(2); /* Spacing */
            DrawDebuggingSection();
            /* Spacing */
            EditorGUILayout.Space(2); /* Spacing */
            DrawGameEventChangerUtilitySection();

            if (GUI.changed) SaveSerializeFields();

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region Main

        protected virtual void DrawMainGameEventData() { }

        protected virtual void DrawDefaultGameEventData() { }

        protected void DrawDebuggingSection()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel++;
            _openDebugFoldout = EditorGUILayout.Foldout(_openDebugFoldout, "Debug");
            EditorGUI.indentLevel--;
            if (_openDebugFoldout)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                _invokeLoggingValue = (bool)_invokeLoggingField.GetValue(_gameEvent);
                _invokeLoggingValue = EditorGUILayout.Toggle("Invoke Logging", _invokeLoggingValue);
                _invokeLoggingField.SetValue(_gameEvent, _invokeLoggingValue);
                if (_invokeLoggingValue)
                {
                    _limitLoggingCountValue = (int)_limitLoggingCountField.GetValue(_gameEvent);
                    _limitLoggingCountValue = EditorGUILayout.IntField("Limit", _limitLoggingCountValue);
                    if (_limitLoggingCountValue < 1)
                        _limitLoggingCountValue = 1;
                    _limitLoggingCountField.SetValue(_gameEvent, _limitLoggingCountValue);
                    _drawLogList.DoLayoutList();
                    /* Spacing */
                    EditorGUILayout.Space(2); /* Spacing */
                }
                _drawSubscribedList.DoLayoutList();
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }

        protected void DrawGameEventChangerUtilitySection()
        {
            void ChooseGameEventType(int index)
            {
                bool confirmed = false;
                ChangeGameEventType(out confirmed);

                if (confirmed)
                    _selectedGameEventTypeName = index;
            }

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("• Change Game Event Utility •");
            GUI.enabled = false;
            GUILayout.TextField(_arrayedAllGameEventTypeNames[_selectedGameEventTypeName]);
            GUI.enabled = true;
            Vector2 popupWindowSize = new Vector2(400f, 200f);
            _popupGameEventTypes = new ScrollableItemPopupContent("Choose Game Event", popupWindowSize,
                _arrayedAllGameEventTypeNames, ChooseGameEventType);
            if (GUILayout.Button("Change Game Event"))
            {
                Rect lastPos = new Rect();
                lastPos.position = Event.current.mousePosition;
                lastPos.size = popupWindowSize;
                PopupWindow.Show(lastPos, _popupGameEventTypes);
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawLogListHeader(Rect rect)
        {
            EditorGUI.indentLevel++;
            bool prev = _foldoutDrawLogList;
            _foldoutDrawLogList = EditorGUI.Foldout(rect, _foldoutDrawLogList, "Invoke Logging");
            if (prev != _foldoutDrawLogList) Repaint();
            rect.position += new Vector2(rect.size.x - 94f, 0f);
            rect.size = new Vector2(96f, 18f);
            if (GUI.Button(rect, "Clear Log", GUI.skin.button))
            {
                _logsValue.Clear();
                _logsField.SetValue(_gameEvent, _logsValue);
            }
            EditorGUI.indentLevel--;
        }

        private void DrawLogListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (!_foldoutDrawLogList) return;

            GUI.enabled = false;
            var log = _logsValue[index];
            float sumHeight = rect.height;
            rect.height = sumHeight * 0.22f;
            rect.y += sumHeight * 0.01f;
            EditorGUI.TextArea(rect, log.reference == null ? "NULL" : log.reference.ToString());
            rect.y += rect.height + sumHeight * 0.02f;
            rect.height = sumHeight * 0.74f;
            EditorGUI.TextArea(rect, string.IsNullOrEmpty(log.extender) ? "NULL" : log.extender);
            GUI.enabled = true;
        }

        private float DrawLogListHeightElement(int index) => !_foldoutDrawLogList ? 0f : 80f;

        private void DrawSubscribedListHeader(Rect rect)
        {
            EditorGUI.indentLevel++;
            bool prev = _foldoutDrawSubscribedList;
            _foldoutDrawSubscribedList = EditorGUI.Foldout(rect, _foldoutDrawSubscribedList, "Subscribed Events");
            if (prev != _foldoutDrawSubscribedList) Repaint();
            EditorGUI.indentLevel--;
        }

        private void DrawSubscribedListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (!_foldoutDrawLogList) return;

            GUI.enabled = false;
            var subs = _subscribedEventsValue[index];
            float sumHeight = rect.height;
            rect.height = sumHeight * 0.22f;
            rect.y += sumHeight * 0.01f;
            EditorGUI.TextArea(rect, subs.reference == null ? "NULL" : subs.reference.ToString());
            rect.y += rect.height + sumHeight * 0.02f;
            rect.height = sumHeight * 0.74f;
            EditorGUI.TextArea(rect, string.IsNullOrEmpty(subs.extender) ? "NULL" : subs.extender);
            GUI.enabled = true;
        }

        private float DrawSubscribedListHeightElement(int index) => !_foldoutDrawSubscribedList ? 0f : 80f;

        /// <summary>
        /// Creates type value dropdown to choose.
        /// </summary>
        protected void GetAllGameEventType()
        {
            var allTypes = typeof(GameEvent).Assembly.GetTypes();
            Type t;
            for (int i = 0; i < allTypes.Length; i++)
            {
                t = allTypes[i];
                if (t.IsAbstract || t.IsGenericTypeDefinition)
                    continue;
                if (!typeof(GameEvent).IsAssignableFrom(t))
                    continue;
                if (_gameEvent.GetType().Name.Equals(t.Name))
                    _selectedGameEventTypeName = _allGameEventType.Count;

                _allGameEventType.Add(t);
                _allGameEventTypeName.Add(t.Name);
            }
        }

        /// <summary>
        /// Save serialize fields when something changed on gui.
        /// </summary>
        protected virtual void SaveSerializeFields()
        {
            _logsValue = (List<GameEvent.Logging>)_logsField.GetValue(_gameEvent);
            _subscribedEventsValue = (List<GameEvent.Logging>)_subscribedEventsField.GetValue(_gameEvent);
            _drawLogList.list = _logsValue;
            _drawSubscribedList.list = _subscribedEventsValue;
            EditorUtility.SetDirty(_gameEvent);
        }

        /// <summary>
        /// Change game event confirmation action.
        /// </summary>
        private void ChangeGameEventType(out bool confirmed)
        {
            confirmed = EditorUtility.DisplayDialog("Change Game Event Type?",
                "Are you sure you want to change this Game Event Type? All references will be removed from all field.",
                "Proceed", "Cancel");

            if (!confirmed) return;

            Selection.activeObject = null;
            var targetPath = AssetDatabase.GetAssetPath(_gameEvent.GetInstanceID());
            AssetDatabase.DeleteAsset(targetPath);

            var newAsset = CreateInstance(_changeEventType);
            AssetDatabase.CreateAsset(newAsset, targetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newAsset;
        }

        #endregion
    }

    /// <summary>
    /// Special editor for game event with no parameter.
    /// </summary>
    [CustomEditor(typeof(GameEventNoParam), true)]
    internal class GameEventNoParam_Editor : GameEvent_Editor
    {
        #region GameEvent_Editor

        protected override void DrawMainGameEventData()
        {
            if (GUILayout.Button(sd.invokeNoParamEventContent))
                _invokeByPlaceholderMethod.Invoke(_gameEvent, null);
        }

        #endregion
    }

    /// <summary>
    /// Editor for abstract game event with one parameter.
    /// </summary>
    [CustomEditor(typeof(GameEvent<>), true)]
    internal class GameEventOneParam_Editor : GameEvent_Editor
    {
        #region Variable

        protected SerializedProperty _defaultParam1Prop = null;
        protected SerializedProperty _param1PlaceholderProp = null;

        #endregion

        #region Editor

        protected override void OnEnable()
        {
            base.OnEnable();
            _defaultParam1Prop = serializedObject.FindProperty("_defaultParam1");
            _param1PlaceholderProp = serializedObject.FindProperty("_param1Placeholder");
        }

        #endregion

        #region GameEvent_Editor

        protected override void DrawMainGameEventData()
        {
            if (_param1PlaceholderProp != null)
                EditorGUILayout.PropertyField(_param1PlaceholderProp, sd.param1PlaceholderContent);
            if (GUILayout.Button(sd.invokeByPlaceholderContent))
                _invokeByPlaceholderMethod.Invoke(_gameEvent, null);
        }

        protected override void DrawDefaultGameEventData()
        {
            if (_defaultParam1Prop != null)
                EditorGUILayout.PropertyField(_defaultParam1Prop, sd.param1DefaultContent);
            if (GUILayout.Button(sd.invokeByDefaultContent))
                _gameEvent.Invoke(_gameEvent);
            if (GUILayout.Button(sd.removeAllListenerContent))
                _gameEvent.RemoveAllListeners();
        }

        #endregion
    }

    /// <summary>
    /// Editor for abstract game event with two parameters.
    /// </summary>
    [CustomEditor(typeof(GameEvent<,>), true)]
    internal class GameEventTwoParam_Editor : GameEventOneParam_Editor
    {
        #region Variable

        protected SerializedProperty _defaultParam2Prop = null;
        protected SerializedProperty _param2PlaceholderProp = null;

        #endregion

        #region Editor

        protected override void OnEnable()
        {
            base.OnEnable();
            _defaultParam2Prop = serializedObject.FindProperty("_defaultParam2");
            _param2PlaceholderProp = serializedObject.FindProperty("_param2Placeholder");
        }

        #endregion

        #region GameEvent_Editor

        protected override void DrawMainGameEventData()
        {
            if (_param1PlaceholderProp != null)
                EditorGUILayout.PropertyField(_param1PlaceholderProp, sd.param1PlaceholderContent);
            if (_param2PlaceholderProp != null)
                EditorGUILayout.PropertyField(_param2PlaceholderProp, sd.param2PlaceholderContent);
            if (GUILayout.Button(sd.invokeByPlaceholderContent))
                _invokeByPlaceholderMethod.Invoke(_gameEvent, null);
        }

        protected override void DrawDefaultGameEventData()
        {
            if (_defaultParam1Prop != null)
                EditorGUILayout.PropertyField(_defaultParam1Prop, sd.param1DefaultContent);
            if (_defaultParam2Prop != null)
                EditorGUILayout.PropertyField(_defaultParam2Prop, sd.param2DefaultContent);
            if (GUILayout.Button(sd.invokeByDefaultContent))
                _gameEvent.Invoke(_gameEvent);
            if (GUILayout.Button(sd.removeAllListenerContent))
                _gameEvent.RemoveAllListeners();
        }

        #endregion
    }

    /// <summary>
    /// Editor for abstract game event with three parameters.
    /// </summary>
    [CustomEditor(typeof(GameEvent<,,>), true)]
    internal class GameEventThreeParam_Editor : GameEventTwoParam_Editor
    {
        #region Variable

        protected SerializedProperty _defaultParam3Prop = null;
        protected SerializedProperty _param3PlaceholderProp = null;

        

        #endregion

        #region Editor

        protected override void OnEnable()
        {
            base.OnEnable();
            _defaultParam3Prop = serializedObject.FindProperty("_defaultParam3");
            _param3PlaceholderProp = serializedObject.FindProperty("_param3Placeholder");
        }

        #endregion

        #region GameEvent_Editor

        protected override void DrawMainGameEventData()
        {
            if (_param1PlaceholderProp != null)
                EditorGUILayout.PropertyField(_param1PlaceholderProp, sd.param1PlaceholderContent);
            if (_param2PlaceholderProp != null)
                EditorGUILayout.PropertyField(_param2PlaceholderProp, sd.param2PlaceholderContent);
            if (_param3PlaceholderProp != null)
                EditorGUILayout.PropertyField(_param3PlaceholderProp, sd.param3PlaceholderContent);
            if (GUILayout.Button(sd.invokeByPlaceholderContent))
                _invokeByPlaceholderMethod.Invoke(_gameEvent, null);
        }

        protected override void DrawDefaultGameEventData()
        {
            if (_defaultParam1Prop != null)
                EditorGUILayout.PropertyField(_defaultParam1Prop, sd.param1DefaultContent);
            if (_defaultParam2Prop != null)
                EditorGUILayout.PropertyField(_defaultParam2Prop, sd.param2DefaultContent);
            if (_defaultParam3Prop != null)
                EditorGUILayout.PropertyField(_defaultParam3Prop, sd.param3DefaultContent);
            if (GUILayout.Button(sd.invokeByDefaultContent))
                _gameEvent.Invoke(_gameEvent);
            if (GUILayout.Button(sd.removeAllListenerContent))
                _gameEvent.RemoveAllListeners();
        }

        #endregion
    }

    /// <summary>
    /// Editor for abstract game event with four parameters.
    /// </summary>
    [CustomEditor(typeof(GameEvent<,,,>), true)]
    internal class GameEventFourParam_Editor : GameEventThreeParam_Editor
    {
        #region Variable

        protected SerializedProperty _defaultParam4Prop = null;
        protected SerializedProperty _param4PlaceholderProp = null;

        #endregion

        #region Editor

        protected override void OnEnable()
        {
            base.OnEnable();
            _defaultParam4Prop = serializedObject.FindProperty("_defaultParam4");
            _param4PlaceholderProp = serializedObject.FindProperty("_param4Placeholder");
        }

        #endregion

        #region GameEvent_Editor

        protected override void DrawMainGameEventData()
        {
            if (_param1PlaceholderProp != null)
                EditorGUILayout.PropertyField(_param1PlaceholderProp, sd.param1PlaceholderContent);
            if (_param2PlaceholderProp != null)
                EditorGUILayout.PropertyField(_param2PlaceholderProp, sd.param2PlaceholderContent);
            if (_param3PlaceholderProp != null)
                EditorGUILayout.PropertyField(_param3PlaceholderProp, sd.param3PlaceholderContent);
            if (_param4PlaceholderProp != null)
                EditorGUILayout.PropertyField(_param4PlaceholderProp, sd.param4PlaceholderContent);
            if (GUILayout.Button(sd.invokeByPlaceholderContent))
                _invokeByPlaceholderMethod.Invoke(_gameEvent, null);
        }

        protected override void DrawDefaultGameEventData()
        {
            if (_defaultParam1Prop != null)
                EditorGUILayout.PropertyField(_defaultParam1Prop, sd.param1DefaultContent);
            if (_defaultParam2Prop != null)
                EditorGUILayout.PropertyField(_defaultParam2Prop, sd.param2DefaultContent);
            if (_defaultParam3Prop != null)
                EditorGUILayout.PropertyField(_defaultParam3Prop, sd.param3DefaultContent);
            if (_defaultParam4Prop != null)
                EditorGUILayout.PropertyField(_defaultParam4Prop, sd.param4DefaultContent);
            if (GUILayout.Button(sd.invokeByDefaultContent))
                _gameEvent.Invoke(_gameEvent);
            if (GUILayout.Button(sd.removeAllListenerContent))
                _gameEvent.RemoveAllListeners();
        }

        #endregion
    }
}
