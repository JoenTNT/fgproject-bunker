using System;
using System.Collections;
using JT.InterfaceScripts;
using UnityEditorInternal;

namespace JT.Editors.Utilities
{
    /// <summary>
    /// Make a limit load reorderable list.
    /// </summary>
    public sealed class LimitReorderableList : ReorderableList, IPagination
    {
        #region Variable

        private IList _fullList = null;
        private Type _elementType = null;

        #endregion

        #region ReorderableList

        public LimitReorderableList(IList elements, Type elementType)
            : base(elements, elementType)
        {
            _fullList = elements;
            _elementType = elementType;
        }

        public LimitReorderableList(IList elements, Type elementType, bool draggable, bool displayHeader, bool displayAddButton, bool displayRemoveButton)
            : base(elements, elementType, draggable, displayHeader, displayAddButton, displayRemoveButton)
        {
            _fullList = elements;
            _elementType = elementType;
        }

        #endregion

        #region IPagination

        public int Page { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int PageCount => throw new NotImplementedException();

        public int PageContentLimit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        #endregion
    }
}
