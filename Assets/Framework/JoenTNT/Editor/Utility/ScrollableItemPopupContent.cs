using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace JT.Editors.Utilities
{
    /// <summary>
    /// An easier scrollable popup control.
    /// </summary>
    public sealed class ScrollableItemPopupContent : PopupWindowContent
    {
        #region Variable

        private string _label = string.Empty;
        private string[] _items = null;
        private Action<int> _callback = null;

        private Vector2 _windowSize = Vector2.zero;
        private Vector2 _scrollPosition = Vector2.zero;

        #endregion

        #region Constructor

        public ScrollableItemPopupContent(Vector2 windowSize, string[] items, Action<int> callback)
        {
            _label = "Choose Item";
            _windowSize = windowSize;
            _items = items;
            _callback = callback;
        }

        public ScrollableItemPopupContent(Vector2 windowSize, List<string> items, Action<int> callback)
        {
            _label = "Choose Item";
            _windowSize = windowSize;
            _items = items.ToArray();
            _callback = callback;
        }

        public ScrollableItemPopupContent(string label, Vector2 windowSize, string[] items, Action<int> callback)
        {
            _label = label;
            _windowSize = windowSize;
            _items = items;
            _callback = callback;
        }

        public ScrollableItemPopupContent(string label, Vector2 windowSize, List<string> items, Action<int> callback)
        {
            _label = label;
            _windowSize = windowSize;
            _items = items.ToArray();
            _callback = callback;
        }

        #endregion

        #region PopupWindowContent

        public override void OnGUI(Rect rect)
        {
            EditorGUILayout.LabelField(_label, EditorStyles.label);
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUI.skin.box);

            for (int i = 0; i < _items.Length; i++)
            {
                int index = i;
                if (GUILayout.Button(_items[i], EditorStyles.textField))
                {
                    _callback?.Invoke(index);
                    editorWindow.Close();
                }
            }

            GUILayout.EndScrollView();
        }

        public override Vector2 GetWindowSize() => _windowSize;

        #endregion

        #region Main

        public float HighestWidthValue()
        {
            return 0f;
        }

        #endregion
    }
}
