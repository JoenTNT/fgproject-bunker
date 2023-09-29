using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace JT
{
    /// <summary>
    /// Editor for parameter container.
    /// </summary>
    [CustomEditor(typeof(Dashboard))]
    [CanEditMultipleObjects]
    public class Dashboard_Editor : Editor, ISerializationCallbackReceiver
    {
        #region Variables

        // Constants.
        private const string PARAM_PAIRS_PROPERTY_PATH = "_paramPairs";
        private const string PARAM_TYPE_PROPERTY_KEY = "_paramType";
        private const string PARAMETER_NAME_PROPERTY_KEY = "_paramName";
        private const string PARAMETER_OBJECT_PROPERTY_KEY = "_parameter";
        private const string FOLDOUT_PARAMETER_KEY = "_foldoutParameter";
        private const string DESTROY_OBJECT_ON_BAKE_KEY = "_destroyObjectOnBake";
        private const string PARAM_OBJ_ISVALID_KEY = "_isValid";
        private const string TARGET_OBJECT_KEY = "_targetObject";
        private static string[] _eligibleTypes = null;
        private static bool _isStaticInit = false;

        // Editor Runtime variable data.
        private ListOfStringPopupWindowContent _typeListContent = null;
        private Dashboard _blackboard = null;
        private List<int> _selectedIndexElementOfTypes = null;

        // Editor drawer properties.
        private GUIStyle _paramNamePlaceholderStyle = null;
        private GUIContent _paramLabelContent = new GUIContent("UNNAMED PARAMETER");
        private GUIContent _paramNamePlaceHolderContent = new GUIContent("Enter Parameter Name...");
        private GUIContent _listTitleLabel = new GUIContent("Parameters");
        private GUIContent _createParamObjBtnContent = new GUIContent("Create");
        private GUIContent _destroyOnBakeContent = new GUIContent("Destroy On Bake");
        private GUIContent _targetObjectContent = new GUIContent("Target");
        private SerializedProperty _listParamProp = null;
        private SerializedProperty _destroyObjectOnBakeProp = null;
        private SerializedProperty _targetObjectProp = null;
        private ReorderableList _reorderParamList = null;

        // Editor temporary variable data.
        private SerializedProperty _tempParamElement = null;
        private SerializedProperty _tempParamElementRtve = null;
        private SerializedProperty _tempParamElementScRtve = null;
        private GUIContent _tempTypeShortNameContent = new GUIContent(GUIContent.none);
        private Object _tempParamObj = null;
        private Object _tempTargetObject = null;

        #endregion

        #region Editor

        private void Awake()
        {
            // Check if statics already initialized.
            if (_isStaticInit) return;

            // Start initialize statics.
            InitStatics();
            _isStaticInit = true;
        }

        private void OnEnable()
        {
            // Get target component.
            _blackboard = (Dashboard)target;

            // Get all references.
            _destroyObjectOnBakeProp = serializedObject.FindProperty(DESTROY_OBJECT_ON_BAKE_KEY);
            _targetObjectProp = serializedObject.FindProperty(TARGET_OBJECT_KEY);

            // Create list of parameters.
            _listParamProp = serializedObject.FindProperty(PARAM_PAIRS_PROPERTY_PATH);
            _reorderParamList = new ReorderableList(serializedObject, _listParamProp, true, true, false, false)
            {
                drawHeaderCallback = ReorderParamList_DrawHeaderCallback,
                drawElementCallback = ReorderParamList_DrawElementCallback,
                elementHeightCallback = ReorderParamList_ElementHeightCallback,
                onReorderCallbackWithDetails = ReorderParamList_OnReorderCallbackWithDetails,
                footerHeight = 0f,
            };

            // Check static initialization.
            if (!_isStaticInit)
            {
                InitStatics();
                _isStaticInit = true;
            }

            // Init list elements.
            _reorderParamList.index = -1;
            _selectedIndexElementOfTypes = new List<int>();
            string typeName = null;
            for (int i = 0; i < _listParamProp.arraySize; i++)
            {
                _tempParamElement = _listParamProp.GetArrayElementAtIndex(i);
                _tempParamElementRtve = _tempParamElement.FindPropertyRelative(PARAM_TYPE_PROPERTY_KEY);
                _selectedIndexElementOfTypes.Add(0);
                typeName = _tempParamElementRtve.stringValue;
                if (string.IsNullOrEmpty(typeName)) continue;
                _selectedIndexElementOfTypes[i] = System.Array.IndexOf(_eligibleTypes, typeName);
            }

            // Create type list content.
            _typeListContent = new ListOfStringPopupWindowContent(_eligibleTypes);
        }

        public override void OnInspectorGUI()
        {
            // Get latest update.
            serializedObject.Update();

            // Start checking changes.
            EditorGUI.BeginChangeCheck();

            // Draw parameters information.
            GUILayout.BeginVertical(GUI.skin.box);
            _tempTargetObject = _targetObjectProp.objectReferenceValue;
            _destroyObjectOnBakeProp.boolValue = EditorGUILayout.Toggle(_destroyOnBakeContent,
                _destroyObjectOnBakeProp.boolValue);
            _targetObjectProp.objectReferenceValue = EditorGUILayout.ObjectField(_targetObjectContent,
                _tempTargetObject, typeof(GameObject), true);
            _reorderParamList.DoLayoutList();
            GUILayout.EndVertical();

            // Check any changes.
            if (EditorGUI.EndChangeCheck())
            {
                // Apply modifications.
                serializedObject.ApplyModifiedProperties();
                _listParamProp.serializedObject.ApplyModifiedProperties();

                // Set dirty.
                EditorUtility.SetDirty(_blackboard);
            }
        }

        #endregion

        #region ISerializationCallbackReceiver

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize() => _isStaticInit = false;

        #endregion

        #region Main

        private void InitStatics()
        {
            // Get all assembly and type and register into lists.
            List<string> ess = new List<string>();
            Assembly[] _assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < _assemblies.Length; i++)
            {
                Assembly _tempAssembly = _assemblies[i];
                System.Type[] _tempTypes = _tempAssembly.GetTypes();

                for (int j = 0; j < _tempTypes.Length; j++)
                {
                    System.Type t = _tempTypes[j];
                    if (t.IsAbstract) continue;
                    if (t.ContainsGenericParameters) continue;
                    if (!t.IsSubclassOf(typeof(Parameter))) continue;

                    ess.Add(t.AssemblyQualifiedName);
                }
            }
            ess.Sort();
            _eligibleTypes = new string[ess.Count];
            ess.CopyTo(_eligibleTypes);
        }

        private void ReorderParamList_DrawHeaderCallback(Rect rect)
        {
            Vector2 textSize = GUI.skin.box.CalcSize(_listTitleLabel);
            Rect labelPos = new Rect((rect.width - textSize.x) / 2f, rect.y, 200f,
                _reorderParamList.headerHeight);
            GUI.Label(labelPos, _listTitleLabel);

            Vector2 buttonSize = GUI.skin.button.CalcSize(new GUIContent("+"));
            rect.x += rect.width - (2 * buttonSize.x);
            rect.width = buttonSize.x;
            if (GUI.Button(rect, "+", GUI.skin.button))
            {
                try { AddNewParameter(_reorderParamList.index); }
                catch (System.ArgumentOutOfRangeException)
                {
                    _reorderParamList.index = -1;
                    AddNewParameter(_reorderParamList.index);
                }
            }

            rect.x += buttonSize.x;
            if (GUI.Button(rect, "-", GUI.skin.button))
                RemoveOldParameter(_reorderParamList.index);
        }

        private void ReorderParamList_DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            // Get parameter element by index.
            _tempParamElement = _reorderParamList.serializedProperty.GetArrayElementAtIndex(index);
            //Debug.Log($"[DEBUG] On Drawing Element {_tempParamElement.displayName} (Type: {_tempParamElement.type})");

            // Draw label of element, and then fold out the rest if active.
            _tempParamElementRtve = _tempParamElement.FindPropertyRelative(PARAMETER_NAME_PROPERTY_KEY);
            _paramLabelContent.text = _tempParamElementRtve.stringValue;
            if (string.IsNullOrEmpty(_paramLabelContent.text))
                _paramLabelContent.text = "UNNAMED PARAMETER";
            rect.height = GUI.skin.box.CalcSize(_listTitleLabel).y;
            rect.x += 10f;
            _tempParamElementRtve = _tempParamElement.FindPropertyRelative(FOLDOUT_PARAMETER_KEY);
            _tempParamElementRtve.boolValue = !EditorGUI.Foldout(rect, !_tempParamElementRtve.boolValue, _paramLabelContent);
            if (_tempParamElementRtve.boolValue) return;
            rect.x -= 10f;

            // Draw choosen type of the parameter.
            _tempParamElementRtve = _tempParamElement.FindPropertyRelative(PARAM_TYPE_PROPERTY_KEY);
            //Debug.Log($"[DEBUG] Draw Element Index: {index}; List Amount: {_selectedIndexElementOfTypes.Count}");
            int selected = _selectedIndexElementOfTypes[index];
            _tempTypeShortNameContent.text = selected < 0 ? "UNSET" : System.Type.GetType(_eligibleTypes[selected]).Name;
            rect.y += GUI.skin.box.CalcSize(_listTitleLabel).y;
            rect.height = GUI.skin.button.CalcSize(_tempTypeShortNameContent).y;
            if (rect.Contains(Event.current.mousePosition))
                EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
            if (GUI.Button(rect, _tempTypeShortNameContent, GUI.skin.button))
            {
                _typeListContent.ReferenceToBeSet = $"{nameof(Dashboard)}.{index}";
                PopupWindow.Show(new Rect(200, 100, 400, 300), _typeListContent);
            }
            //Debug.Log($"[DEBUG] Ref: {_typeListContent.ReferenceToBeSet}; Is Choosen: {_typeListContent.IsChoosen}");
            if (!string.IsNullOrEmpty(_typeListContent.ReferenceToBeSet) && _typeListContent.IsChoosen)
            {
                string[] chooseRef = _typeListContent.ReferenceToBeSet.Split('.');
                int i = int.Parse(chooseRef[1]);
                //Debug.Log($"[DEBUG] Change Reference at index {index} to {_typeListContent.ChoosenString} (I is {i})");
                if (i != index) goto SkipOnWrongIndex;

                selected = _typeListContent.ChoosenIndex;
                _typeListContent.ReferenceToBeSet = null;
                _typeListContent.ResetChoosen();
            }
        SkipOnWrongIndex:
            _selectedIndexElementOfTypes[index] = selected;
            string type = null;
            if (selected >= 0)
            {
                type = _eligibleTypes[selected];
                _tempParamElementRtve.stringValue = type;
                _tempParamElementRtve.serializedObject.ApplyModifiedProperties();
            }

            // Draw parameter name field.
            Vector2 titleLabelSize = GUI.skin.textField.CalcSize(_listTitleLabel);
            _tempParamElementRtve = _tempParamElement.FindPropertyRelative(PARAMETER_NAME_PROPERTY_KEY);
            string paramName = _tempParamElementRtve.stringValue;
            rect.y += titleLabelSize.y;
            _tempParamElementRtve.stringValue = GUI.TextField(rect, paramName, 64, GUI.skin.textField);
            if (_paramNamePlaceholderStyle == null)
            {
                _paramNamePlaceholderStyle = new GUIStyle();
                _paramNamePlaceholderStyle.fontStyle = FontStyle.Italic;
                _paramNamePlaceholderStyle.normal.textColor = Color.gray;
                _paramNamePlaceholderStyle.alignment = TextAnchor.MiddleLeft;
                _paramNamePlaceholderStyle.padding = new RectOffset(4, 2, 0, 0);
            }
            if (rect.Contains(Event.current.mousePosition))
                EditorGUIUtility.AddCursorRect(rect, MouseCursor.Text);
            if (string.IsNullOrEmpty(paramName))
                GUI.Label(rect, _paramNamePlaceHolderContent, _paramNamePlaceholderStyle);

            // Draw the value of parameter.
            _tempParamElementRtve = _tempParamElement.FindPropertyRelative(PARAMETER_OBJECT_PROPERTY_KEY);
            _tempParamObj = _tempParamElementRtve.objectReferenceValue;
            System.Type typeObj = System.Type.GetType(type);
            rect.y += titleLabelSize.y;
            float btnWidth = rect.width / 4f;
            if (btnWidth < 80f) btnWidth = 80f;
            bool isTypeMatch = _tempParamObj?.GetType().AssemblyQualifiedName == type;
            bool isInvalidParamObj = _tempParamObj == null || !isTypeMatch;
            string hintMessage = _tempParamObj == null ? "Object is Not Initialized"
                : isTypeMatch ? null : $"Should {typeObj.Name}";
            _tempParamElementScRtve = _tempParamElement.FindPropertyRelative(PARAM_OBJ_ISVALID_KEY);
            _tempParamElementScRtve.boolValue = !isInvalidParamObj;

            if (!isInvalidParamObj) goto SkipCreateOrReplace;

            Rect warningRect = new Rect(rect.x, rect.y, rect.width, 32f);
            EditorGUI.HelpBox(warningRect, "Please Resolve this immediately...", MessageType.Error);
            rect.y += warningRect.height;

            Rect createBtnRect = new Rect(rect.x, rect.y, btnWidth, titleLabelSize.y);
            _createParamObjBtnContent.text = _tempParamObj == null ? "Create" : "Replace";
            if (!string.IsNullOrEmpty(hintMessage))
            {
                Rect hintLabelRect = new Rect(rect.x + btnWidth, rect.y, rect.width * 3f / 4f, titleLabelSize.y);
                GUI.Label(hintLabelRect, hintMessage);
            }
            rect.y += createBtnRect.height;

            if (!GUI.Button(createBtnRect, _createParamObjBtnContent)) goto SkipCreateOrReplace;

            System.Type projectWinUtilType = typeof(ProjectWindowUtil);
            MethodInfo getActiveFolderPath = projectWinUtilType.GetMethod(
                "GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
            object activeFolderPathObj = getActiveFolderPath.Invoke(null, new object[0]);
            string pwpath = activeFolderPathObj.ToString();

            _tempParamObj = CreateInstance(typeObj);
            _tempParamObj.name = $"New {typeObj.Name} {Directory.GetFiles(pwpath).Length}";
            string soFilename = Path.Combine(pwpath, $"{_tempParamObj.name}.asset");
            AssetDatabase.CreateAsset(_tempParamObj, soFilename);
            AssetDatabase.SaveAssets();
        SkipCreateOrReplace:
            _tempParamObj = EditorGUI.ObjectField(rect, _tempParamObj, typeObj, false);
            _tempParamElementRtve.objectReferenceValue = _tempParamObj;
            _tempParamElementRtve.serializedObject.ApplyModifiedProperties();
        }

        private float ReorderParamList_ElementHeightCallback(int index)
        {
            float height = 0f;
            _tempParamElement = _reorderParamList.serializedProperty.GetArrayElementAtIndex(index);
            bool isNotFoldout = _tempParamElement.FindPropertyRelative(FOLDOUT_PARAMETER_KEY).boolValue;
            if (!isNotFoldout)
            {
                float inspectorWidth = EditorGUIUtility.labelWidth + 30f;
                height += GUI.skin.box.CalcHeight(_paramLabelContent, inspectorWidth);
                height += GUI.skin.button.CalcHeight(_listTitleLabel, inspectorWidth);
                height += GUI.skin.textField.CalcHeight(_paramNamePlaceHolderContent, inspectorWidth);
                if (!_tempParamElement.FindPropertyRelative(PARAM_OBJ_ISVALID_KEY).boolValue)
                    height += 32f + GUI.skin.button.CalcHeight(_listTitleLabel, inspectorWidth);
                height += EditorStyles.objectField.CalcHeight(_paramNamePlaceHolderContent, inspectorWidth);
            }
            else height = 16f;
            return height;
        }

        private void ReorderParamList_OnReorderCallbackWithDetails(ReorderableList list, int oldIndex, int newIndex)
        {
            //Debug.Log($"[DEBUG] Change from Old Index ({oldIndex}) to New Index ({newIndex})");
            var oldElementType = _selectedIndexElementOfTypes[oldIndex];
            _selectedIndexElementOfTypes.RemoveAt(oldIndex);
            _selectedIndexElementOfTypes.Insert(newIndex, oldElementType);
        }

        private void AddNewParameter(int selectedIndex)
        {
            if (selectedIndex < 0) selectedIndex = _selectedIndexElementOfTypes.Count;
            _selectedIndexElementOfTypes.Insert(selectedIndex, 0);
            _listParamProp.InsertArrayElementAtIndex(selectedIndex);
        }

        private void RemoveOldParameter(int selectedIndex)
        {
            if (selectedIndex == -1) selectedIndex = _selectedIndexElementOfTypes.Count;
            if (selectedIndex >= _selectedIndexElementOfTypes.Count)
                selectedIndex = _selectedIndexElementOfTypes.Count - 1;
            if (_selectedIndexElementOfTypes.Count == 0) return;
            _selectedIndexElementOfTypes.RemoveAt(selectedIndex);
            _listParamProp.DeleteArrayElementAtIndex(selectedIndex);
        }

        #endregion
    }
}
