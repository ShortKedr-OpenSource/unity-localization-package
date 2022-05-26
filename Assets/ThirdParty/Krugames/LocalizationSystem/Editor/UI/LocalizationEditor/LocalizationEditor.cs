using System;
using System.Collections.Generic;
using Krugames.LocalizationSystem.Common.Editor.UnityInternal;
using Krugames.LocalizationSystem.Editor.Styles;
using Krugames.LocalizationSystem.Editor.UI.LocalizationEditor.Functions;
using Krugames.LocalizationSystem.Editor.UI.LocalizationEditor.PlainView;
using Krugames.LocalizationSystem.Implementation;
using Krugames.LocalizationSystem.Models;
using Krugames.LocalizationSystem.Models.Structs;
using Krugames.LocalizationSystem.Models.Terms;
using Krugames.LocalizationSystem.Models.Utility.Editor;
using Krugames.LocalizationSystem.RapidStorage.Servers;
using ThirdParty.Krugames.LocalizationSystem.Editor.UI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


//TODO add NoteServer
namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor {
    
    /// <summary>
    /// Localization Package main editor.
    /// </summary>
    public class LocalizationEditor : EditorWindow {

        private enum ViewMode {
            Plain = 0,
            Grouping = 1,
        }

        private static class EditorSaveKeys {
            public const string CreationMarkKey = "Krugames_LocalizationEditor_CreationMark";
            public const string ViewModeKey = "Krugames_LocalizationEditor_ViewMode";
            public const string LeftPanelStateKey = "Krugames_Localization_LeftPanelState";
            public const string RightPanelStateKey = "Krugames_Localization_RightPanelState";
        }

        private const string LeftPanelToggleClassName = "LeftPanel";
        private const string RightPanelToggleClassName = "RightPanel";
        private const string AddLocaleButtonClassName = "LeftPanel_AddLocaleButton";
        
        private static readonly Vector2 MinSize = new Vector2(1000, 500);
        private static readonly Vector2 DefaultSize = new Vector2(1000, 500);
        
        [SerializeField] private ViewMode viewMode;
        [SerializeField] private bool leftPanelState;
        [SerializeField] private bool rightPanelState;

        private VisualElement _root;
        private ElementToolbar _toolbar;
        private TwoSidePanelView _content;

        #region ToolbarElements
        private ToggleButton _leftPanelToggleButton;
        private ToggleButton _rightPanelToggleButton;
        private EnumButton<ViewMode> _viewModeSelector;
        private Button _validationButton;
        private Button _translationButton;
        #endregion

        #region ContentElements
        private PlainLocaleEditor _plainLocaleEditor;
        private GroupsView _groupsView;
        #endregion

        #region LeftPanelElements
        private LocaleParamEditor _localeParamEditor;
        private LocaleList _localeList;
        private Button _addLocaleButton;
        #endregion

        #region RightPanelElements
        private FunctionGroupsList _functionGroupsList;
        #endregion

        private FunctionGroup<string> _termFunctionGroup;
        private FunctionElement<TermOpenPropertiesFunction> _termPropertiesFunction;
        private FunctionElement<StringTermTranslateFunction> _termTranslateFunction;

        private OptionPopup _managedLocaleBaseOptionPopup;
        private OptionPopup _managedLocaleOptionPopup;
        private OptionPopup _validationOptionPopup;
        private OptionPopup _translationOptionPopup;
        
        private SerializedObject _serializedObject;
        private LocaleLibrary _localeLibrary;
        private LocaleListElement _managedLocaleListElement;

        [MenuItem("Window/Krugames/Localization")]
        public static void Open() {
            LocalizationEditor editorWindow = EditorWindow.GetWindow<LocalizationEditor>("Localization", true);
            editorWindow.minSize = MinSize;
            if (!EditorPrefs.HasKey(EditorSaveKeys.CreationMarkKey)) {
                editorWindow.position = new Rect(editorWindow.position.position, DefaultSize);
                EditorPrefs.SetBool(EditorSaveKeys.CreationMarkKey, true);
            }
            editorWindow.Show();
            var a = TermNoteServer.Instance;
            TermNoteServer.SetNote("my_term", "my note");
        }

        private void Awake() {
            this.SetAntiAliasing(4);
            LoadEditorValues();
        }

        private void OnDestroy() {
            SaveEditorValues();
            LocaleLibraryUtility.SortStaticLocales();
        }

        public void CreateGUI() {
            _serializedObject = new SerializedObject(this);
            _localeLibrary = LocaleLibrary.Instance;
            _managedLocaleListElement = null;
            if (_localeLibrary.BaseLocale == null) LocaleLibraryUtility.AddLocaleToLibrary(SystemLanguage.English);

            _root = rootVisualElement;
            _root.styleSheets.Add(LocalizationEditorStyles.LocalizationEditorStyle);

            _toolbar = new ElementToolbar();
            _content = new TwoSidePanelView(leftPanelState, rightPanelState);
            
            _root.Add(_toolbar);
            _root.Add(_content);

            CreateToolbarUI();
            CreateContentUI();
            CreateLeftPanelUI();
            CreateRightPanelUI();

            _localeParamEditor.OnChange += Event_LocaleParamEditorChange;
            _localeList.OnSelect += Event_OnLocaleSelect;
            _localeList.OnPropertiesClick += Event_LocalePropertiesClick;
            _plainLocaleEditor.OnTermSelect += Event_PlainEditorTermSelect;
            
            _viewModeSelector.RegisterCallback<ChangeEvent<ViewMode>>(Event_ViewModeChanged);
            
            InitializePopups();
            InitializeFunctions();
            SetupLocaleList();
        }

        private void CreateToolbarUI() {
            _leftPanelToggleButton = new ToggleButton(leftPanelState, Event_LeftPanelStateChange) {text = "L"};
            _leftPanelToggleButton.AddToClassList(LeftPanelToggleClassName);

            _rightPanelToggleButton = new ToggleButton(rightPanelState, Event_RightPanelStateChange) {text = "F"};
            _rightPanelToggleButton.AddToClassList(RightPanelToggleClassName);

            _viewModeSelector = new EnumButton<ViewMode>(viewMode);
            _viewModeSelector.BindProperty(_serializedObject.FindProperty("viewMode"));

            _validationButton = new Button(Event_ValidationButtonClick) {text = "Validate"};
            _translationButton = new Button(Event_TranslationButtonClick) {text = "Translate"};
            
            _toolbar.LeftAnchor.Add(_leftPanelToggleButton);
            _toolbar.LeftAnchor.Add(new ToolbarSpacer());
            _toolbar.LeftAnchor.Add(_validationButton);
            _toolbar.LeftAnchor.Add(_translationButton);

            _toolbar.RightAnchor.Add(_rightPanelToggleButton);
            _toolbar.RightAnchor.Add(new ToolbarSpacer());
            _toolbar.RightAnchor.Add(_viewModeSelector);
        }

        private void CreateContentUI() {
            _plainLocaleEditor = new PlainLocaleEditor(null);
            _groupsView = new GroupsView(null);
            UpdateCurrentView();
        }
        
        private void CreateLeftPanelUI() {
            _localeParamEditor = new LocaleParamEditor("Selected locale", null);
            _localeList = new LocaleList("Locales", null);

            _addLocaleButton = new Button(Event_AddLocaleButtonClick) {text = "Add locale"};
            _addLocaleButton.AddToClassList(AddLocaleButtonClassName);
            
            _content.LeftPanel.Add(_localeParamEditor);
            _content.LeftPanel.Add(_localeList);
            _content.LeftPanel.Add(_addLocaleButton);
        }

        private void CreateRightPanelUI() {
            _functionGroupsList = new FunctionGroupsList("Functions");
            _content.RightPanel.Add(_functionGroupsList);
        }
        
        private void InitializePopups() {
            
            _translationOptionPopup = new OptionPopup(new SelectorListPopup.Element[] {
                new SelectorListPopup.Element("From base to others", () => Debug.LogWarning("\"From base to others\" feature not available in current version")),
                new SelectorListPopup.Element("From one to another", () => Debug.LogWarning("\"From one to another\"This feature not available in current version")),
            }); 
            
            _validationOptionPopup = new OptionPopup(new SelectorListPopup.Element[] {
                new SelectorListPopup.Element("Language duplicates check", () => Debug.Log("1")),
                new SelectorListPopup.Element("Term duplicates check", () => Debug.Log("2")),
                new SelectorListPopup.Element("Layout consistency check", () => Debug.Log("3")),
                new SelectorListPopup.Element("Empty field check", () => Debug.Log("4")),
            });
            
            _managedLocaleBaseOptionPopup = new OptionPopup(new SelectorListPopup.Element[] {
                new SelectorListPopup.Element("Properties", OpenManagedLocaleProperties),
            });
            
            _managedLocaleOptionPopup = new OptionPopup(new SelectorListPopup.Element[] {
                new SelectorListPopup.Element("Properties", OpenManagedLocaleProperties),
                new SelectorListPopup.Element("Remove", RemoveManagedLocale)
            });
        }

        private void InitializeFunctions() {
            _termPropertiesFunction =
                new FunctionElement<TermOpenPropertiesFunction>(TermOpenPropertiesFunction.Invalid, 0);
            _termTranslateFunction = 
                new FunctionElement<StringTermTranslateFunction>(StringTermTranslateFunction.Invalid, 1);
            
            _termFunctionGroup = new FunctionGroup<string>(string.Empty,
                new FunctionElement[] {
                    _termPropertiesFunction,
                    _termTranslateFunction,
                }, 0);
            
            _functionGroupsList.AddFunctionGroup(
                _termFunctionGroup
                );
        }

        private void UpdateCurrentView() {
            _plainLocaleEditor.RemoveFromHierarchy();
            _groupsView.RemoveFromHierarchy();
            switch (viewMode) {
                case ViewMode.Plain:
                    _content.Content.Add(_plainLocaleEditor);
                    break;
                case ViewMode.Grouping:
                    _content.Content.Add(_groupsView);
                    break;
            }
        }

        private void SetupLocaleList() {
            var baseLocale = _localeLibrary.BaseLocale;
            var staticLocales = _localeLibrary.StaticLocales;
            List<Locale> allStaticLocales = new List<Locale>(1 + staticLocales.Length);
            allStaticLocales.Add(baseLocale);
            allStaticLocales.AddRange(staticLocales);
            SetLocales(allStaticLocales.ToArray());
        }
        
        private void SetLocales(Locale[] locales) {
            _localeParamEditor.SetLocale(null);
            _localeList.SetLocales(locales);

            var baseStaticLocale = _localeLibrary.BaseLocale;
            var listElements = _localeList.ListElements;
            for (int i = 0; i < listElements.Length; i++) {
                if (listElements[i].Locale == baseStaticLocale) {
                    listElements[i].SetCustomLabel(baseStaticLocale.name + " (Base)");
                    break;
                }
            }
        }

        private void UpdateLocaleList() {
            var listElements = _localeList.ListElements;
            var baseStaticLocale = _localeLibrary.BaseLocale;
            for (int i = 0; i < listElements.Length; i++) {
                if (listElements[i].Locale == baseStaticLocale) {
                    listElements[i].SetCustomLabel(baseStaticLocale.name + " (Base)");
                    break;
                }
            }
            _localeList.UpdateValues();
        }

        private void OpenManagedLocaleProperties() {
            if (_managedLocaleListElement == null || _managedLocaleListElement.Locale == null) return;
            EditorInternalUtility.OpenPropertyEditor(_managedLocaleListElement.Locale);
        }
        
        private void RemoveManagedLocale() {
            if (_managedLocaleListElement == null || _managedLocaleListElement.Locale == null) return;
            bool result = LocaleLibraryUtility.RemoveLocaleFromLibrary(_managedLocaleListElement.Locale);
            if (result) SetupLocaleList();
        }

        private void SetLocaleToPlainView(Locale locale) {
            var baseStaticLocale = _localeLibrary.BaseLocale;
            if (locale == null) {
                _plainLocaleEditor.SetLocale(null);
                _plainLocaleEditor.CanAddTerm = false;
                _plainLocaleEditor.CanRemoveTerm = false;
            }
            else {
                if (baseStaticLocale == locale) {
                    _plainLocaleEditor.CanAddTerm = true;
                    _plainLocaleEditor.CanRemoveTerm = true;
                } else {
                    _plainLocaleEditor.CanAddTerm = false;
                    _plainLocaleEditor.CanRemoveTerm = false;
                    TermStructureInfo[] layout = _localeLibrary.BaseLocale.GetLayout();
                    LocaleUtility.SetLayout(locale, layout);
                }
                _plainLocaleEditor.SetLocale(locale);
            }
        }

        

        private void SetDefaultEditorValues() {
            viewMode = ViewMode.Plain;
            leftPanelState = true;
            rightPanelState = false;
        }
        
        private void SaveEditorValues() {
            EditorPrefs.SetString(EditorSaveKeys.ViewModeKey, viewMode.ToString());
            EditorPrefs.SetBool(EditorSaveKeys.LeftPanelStateKey, (leftPanelState = _content.ShowLeftPanel));
            EditorPrefs.SetBool(EditorSaveKeys.RightPanelStateKey, (rightPanelState = _content.ShowRightPanel));
        }

        private void LoadEditorValues() {
            SetDefaultEditorValues();
            
            if (EditorPrefs.HasKey(EditorSaveKeys.ViewModeKey)) {
                string value = EditorPrefs.GetString(EditorSaveKeys.ViewModeKey);
                Enum.TryParse(value, out viewMode);
            }

            if (EditorPrefs.HasKey(EditorSaveKeys.LeftPanelStateKey)) {
                leftPanelState = EditorPrefs.GetBool(EditorSaveKeys.LeftPanelStateKey);
            }

            if (EditorPrefs.HasKey(EditorSaveKeys.RightPanelStateKey)) {
                rightPanelState = EditorPrefs.GetBool(EditorSaveKeys.RightPanelStateKey);
            }
        }
        
        private void Event_ViewModeChanged(ChangeEvent<ViewMode> evt) {
            UpdateCurrentView();
        }
        
        private void Event_PlainEditorTermSelect(PlainLocaleEditor self, LocaleTerm localeTerm) {
            if (viewMode != ViewMode.Plain) return;

            if (localeTerm is StringTerm toTerm) {
                var baseLocale = _localeLibrary.BaseLocale;
                
                StringTerm fromTerm = (baseLocale != null) ? baseLocale.GetTerm<StringTerm>(localeTerm.Term) : null;
                SystemLanguage fromLanguage = (baseLocale != null) ? baseLocale.Language : default;
                SystemLanguage toLanguage = (_plainLocaleEditor.Locale != null) ? _plainLocaleEditor.Locale.Language : default;
                
                StringTermTranslateFunction function =
                    new StringTermTranslateFunction(fromTerm, fromLanguage, toTerm, toLanguage);
                _termTranslateFunction.SetFunction(function);
            } else {
                _termTranslateFunction.SetFunction(StringTermTranslateFunction.Invalid);
            }
            
            _termPropertiesFunction.SetFunction(new TermOpenPropertiesFunction(localeTerm));
            _termFunctionGroup.SetSource(localeTerm.Term);
            
            _functionGroupsList.Update();
        }

        private void Event_AddLocaleButtonClick() {
            Locale locale = LocaleLibraryUtility.AddLocaleToLibrary(SystemLanguage.Unknown);
            if (locale != null) {
                TermStructureInfo[] layout = _localeLibrary.BaseLocale.GetLayout();
                LocaleUtility.SetLayout(locale, layout);
                SetupLocaleList();
            }
        }
        
        private void Event_LocaleParamEditorChange(LocaleParamEditor self) {
            if (self.Locale != null) {
                LocaleLibraryUtility.RenameLocaleAssetToLanguageMatch(self.Locale);
                UpdateLocaleList();
            }
        }

        private void Event_LocalePropertiesClick(LocaleList self, LocaleListElement selectedElement) {
            _managedLocaleListElement = selectedElement;
            if (_managedLocaleListElement == null || _managedLocaleListElement.Locale == null) return;
            var baseStaticLocale = _localeLibrary.BaseLocale;
            var popup = (_managedLocaleListElement.Locale == baseStaticLocale)
                ? _managedLocaleBaseOptionPopup
                : _managedLocaleOptionPopup; 
            Rect mouseRect = new Rect(Event.current.mousePosition, Vector2.one); 
            UnityEditor.PopupWindow.Show(mouseRect, popup);
        }

        private void Event_OnLocaleSelect(LocaleList self, LocaleList.SelectionInfo selectionInfo) {
            _localeParamEditor.SetLocale(selectionInfo.Locale);
            switch (viewMode) {
                case ViewMode.Plain:
                    SetLocaleToPlainView(selectionInfo.Locale);
                    break;
                case ViewMode.Grouping:
                    SetLocaleToPlainView(selectionInfo.Locale);
                    break;
            }
        }
        
        private void Event_LeftPanelStateChange(ChangeEvent<bool> evt) {
            leftPanelState = evt.newValue;
            _content.ShowLeftPanel = evt.newValue;
        }
        
        private void Event_RightPanelStateChange(ChangeEvent<bool> evt) {
            rightPanelState = evt.newValue;
            _content.ShowRightPanel = evt.newValue;
        }

        private void Event_ValidationButtonClick() {
            Rect buttonRect = _validationButton.worldBound;
            buttonRect.width = 0;
            UnityEditor.PopupWindow.Show(buttonRect, _validationOptionPopup);
        }
        
        private void Event_TranslationButtonClick() {
            Rect buttonRect = _translationButton.worldBound;
            buttonRect.width = 0;
            UnityEditor.PopupWindow.Show(buttonRect, _translationOptionPopup);
        }

    }
}