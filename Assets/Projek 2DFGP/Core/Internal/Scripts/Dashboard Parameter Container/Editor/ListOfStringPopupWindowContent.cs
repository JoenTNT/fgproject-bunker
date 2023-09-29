using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace JT
{
    /// <summary>
    /// Special case to draw popup window when selecting huge amount of data types in project.
    /// </summary>
    public class ListOfStringPopupWindowContent : PopupWindowContent
    {
        #region Variables

        /// <summary>
        /// Event called when one of the element has been choosen.
        /// </summary>
        public event System.Action<string> OnChooseStr;

        private static readonly GUIContent SEARCH_BAR_CONTENT = new GUIContent("Enter Query...");

        // Editor runtime variable data.
        private string _refToSet = null;
        private string[] _strs = null;
        private List<string> _searchedStrs = null;
        private string _searchQuery = string.Empty;
        private bool _isChoosen = false;
        private string _choosenStr = null;
        private int _choosenIndex = -1;

        // Editor temporary variable data.
        private List<Rect> _elementPos = null;
        private GUIStyle _listElementStyle = null;
        private GUIContent oneLineContent = new GUIContent();
        private Vector2 _scrollPos = Vector2.zero;
        private float _contentHeight = 0f;
        private bool _isInit = false;
        private bool _isQuerySearchChanged = false;

        #endregion

        #region Properties

        /// <summary>
        /// Choosen reference to be set.
        /// </summary>
        public string ReferenceToBeSet
        {
            get => _refToSet;
            set => _refToSet = value;
        }

        /// <summary>
        /// Choosen string value.
        /// </summary>
        public string ChoosenString => _choosenStr;

        /// <summary>
        /// Choosen index on value.
        /// </summary>
        public int ChoosenIndex => _choosenIndex;

        /// <summary>
        /// Is choosen status.
        /// </summary>
        public bool IsChoosen => _isChoosen;

        #endregion

        #region Constructor

        public ListOfStringPopupWindowContent(string[] typeList)
        {
            _strs = typeList;
            _elementPos = new List<Rect>();
            _searchedStrs = new List<string>();
            _isQuerySearchChanged = true;
            _isInit = false;
        }

        //public ListOfStringPopupWindowContent(System.Type[] typeList)
        //{
        //    _typeListStr = new string[typeList.Length];
        //    for (int i = 0; i < typeList.Length; i++)
        //        _typeListStr[i] = typeList[i].FullName;
        //}

        #endregion

        #region Window

        public override Vector2 GetWindowSize() => new Vector2(360f, 240f);

        public override void OnOpen() => ResetChoosen();

        public override void OnGUI(Rect rect)
        {
            // Create a search bar.
            Vector2 searchBarContent = EditorStyles.toolbarSearchField.CalcSize(SEARCH_BAR_CONTENT);
            Rect searchBarRect = new Rect(0f, 0f, rect.width, searchBarContent.y);
            string query = GUI.TextField(searchBarRect, _searchQuery, EditorStyles.toolbarSearchField);
            if (query != _searchQuery)
            {
                _isQuerySearchChanged = true;
                _searchQuery = query;
            }

            // List all types here using scroller.
            Rect scrollContent = new Rect(0f, searchBarRect.height, rect.width, rect.height - searchBarContent.y);
            Vector2 vertScrollBarSize = GUI.skin.verticalScrollbar.CalcSize(GUIContent.none);
            Rect viewScrollRect = new Rect(0f, searchBarRect.height, rect.width - vertScrollBarSize.x, scrollContent.y);
            viewScrollRect.height = _contentHeight;
            _scrollPos = GUI.BeginScrollView(scrollContent, _scrollPos, viewScrollRect, false, true,
                GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar);

            if (_listElementStyle == null)
            {
                _listElementStyle = new GUIStyle(GUI.skin.button);
                _listElementStyle.alignment = TextAnchor.MiddleLeft;
                _listElementStyle.padding = new RectOffset(4, 2, 0, 0);
                _listElementStyle.wordWrap = true;
            }

            if (!_isInit || _isQuerySearchChanged)
            {
                _elementPos.Clear();
                _searchedStrs.Clear();

                Rect btnPos = new Rect(0f, searchBarRect.height, viewScrollRect.width, 0f);
                float viewHeight = 0f, wordWrapHeight;
                
                for (int i = 0; i < _strs.Length; i++)
                {
                    if (!string.IsNullOrEmpty(_searchQuery) && !_strs[i].Contains(_searchQuery))
                        continue;

                    _searchedStrs.Add(_strs[i]);
                    oneLineContent.text = _strs[i];
                    wordWrapHeight = _listElementStyle.CalcHeight(oneLineContent, btnPos.width);
                    btnPos.height = wordWrapHeight < 24f ? 24f : wordWrapHeight;
                    _elementPos.Add(btnPos);
                    btnPos.y += btnPos.height;
                    viewHeight += wordWrapHeight;
                }

                _contentHeight = viewHeight;
                _isInit = true;
                _isQuerySearchChanged = false;
            }
            else
            {
                for (int i = 0; i < _searchedStrs.Count; i++)
                {
                    _isChoosen = GUI.Button(_elementPos[i], _searchedStrs[i], _listElementStyle);
                    if (_isChoosen)
                    {
                        _choosenStr = _searchedStrs[i];
                        _choosenIndex = System.Array.IndexOf(_strs, _choosenStr);
                        break;
                    }
                }
            }

            GUI.EndScrollView();

            // Check choosen string value.
            if (_isChoosen)
            {
                OnChooseStr?.Invoke(_choosenStr);
                editorWindow.Close();
            }
        }

        #endregion

        #region Main

        public void ResetChoosen()
        {
            _isChoosen = false;
            _choosenStr = null;
            _choosenIndex = -1;
        }

        #endregion
    }
}

