using System;
using System.Collections.Generic;
using Krugames.LocalizationSystem.Common.Editor.UnityInternal;
using Krugames.LocalizationSystem.Editor.Package;
using Krugames.LocalizationSystem.Editor.Styles;
using Krugames.LocalizationSystem.Implementation;
using Krugames.LocalizationSystem.Models;
using Krugames.LocalizationSystem.Models.Structs;
using Krugames.LocalizationSystem.Models.Utility.Editor;
using ThirdParty.Krugames.LocalizationSystem.Editor.UI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


//TODO encapsulate leftPanel, rightPanel, + view to TwoSidePanelView class
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
            public const string CreationMark = "Krugames_LocalizationEditor_CreationMark";
            public const string ViewMode = "Krugames_LocalizationEditor_ViewMode";
            public const string LeftPanelState = "Krugames_Localization_LeftPanelState";
            public const string RightPanelState = "Krugames_Localization_RightPanelState";
        }

        private const string LeftPanelToggleClassName = "LeftPanel";
        private const string RightPanelToggleClassName = "RightPanel";
        
        private static readonly Vector2 MinSize = new Vector2(1000, 500);
        private static readonly Vector2 DefaultSize = new Vector2(1000, 500);
        
        [SerializeField] private ViewMode viewMode;
        [SerializeField] private bool leftPanelState;
        [SerializeField] private bool rightPanelState;
        
        private LocaleLibrary _localeLibrary;

        
        private VisualElement _root;

        private ElementToolbar _toolbar;
        private ToggleButton _leftPanelToggleButton;
        private ToggleButton _rightPanelToggleButton;
        private EnumButton<ViewMode> _viewModeSelector;
        private Button _validationButton;
        private Button _translationButton;
        
        private TwoSidePanelView _content;
        
        private LocaleParamEditor _localeParamEditor;
        private LocaleList _localeList;

        private OptionPopup _validationOptionPopup;
        private OptionPopup _translationOptionPopup;

        private LocaleListElement _managedLocaleListElement;
        private OptionPopup _managedLocaleBaseOptionPopup;
        private OptionPopup _managedLocaleOptionPopup;
        

        [MenuItem("Window/Krugames/Localization")]
        public static void Open() {
            LocalizationEditor editorWindow = EditorWindow.GetWindow<LocalizationEditor>("Localization", true);
            editorWindow.minSize = MinSize;
            if (!EditorPrefs.HasKey(EditorSaveKeys.CreationMark)) {
                editorWindow.position = new Rect(editorWindow.position.position, DefaultSize);
                EditorPrefs.SetBool(EditorSaveKeys.CreationMark, true);
            }
            editorWindow.Show();
        }
        
        private void OnEnable() {
            this.SetAntiAliasing(4);
        }
        
        private void Awake() {
            _localeLibrary = LocaleLibrary.Instance;
            if (_localeLibrary.BaseLocale == null) LocaleLibraryUtility.AddLocaleToLibrary(SystemLanguage.English);
            LoadEditorValues();
        }

        private void OnDestroy() {
            SaveEditorValues();
            LocaleLibraryUtility.SortLocaleLibraryStaticLocales();
        }

        public void CreateGUI() {

            SerializedObject serializedObject = new SerializedObject(this);

            _root = rootVisualElement;
            _root.styleSheets.Add(LocalizationEditorStyles.LocalizationEditorStyle);

            #region Toolbar
            _toolbar = new ElementToolbar();

            _leftPanelToggleButton = new ToggleButton(leftPanelState, LeftPanelStateChangeEvent) {text = "L"};
            _leftPanelToggleButton.AddToClassList(LeftPanelToggleClassName);

            _rightPanelToggleButton = new ToggleButton(rightPanelState, RightPanelStateChangeEvent) {text = "F"};
            _rightPanelToggleButton.AddToClassList(RightPanelToggleClassName);

            _viewModeSelector = new EnumButton<ViewMode>(viewMode);
            _viewModeSelector.BindProperty(serializedObject.FindProperty("viewMode"));

            _validationButton = new Button(ValidationButtonClickEvent) {text = "Validation"};
            _translationButton = new Button(TranslationButtonClickEvent) {text = "Translation"};
            
            _toolbar.LeftAnchor.Add(_leftPanelToggleButton);
            _toolbar.LeftAnchor.Add(new ToolbarSpacer());
            _toolbar.LeftAnchor.Add(_validationButton);
            _toolbar.LeftAnchor.Add(_translationButton);

            _toolbar.RightAnchor.Add(_rightPanelToggleButton);
            _toolbar.RightAnchor.Add(new ToolbarSpacer());
            _toolbar.RightAnchor.Add(_viewModeSelector);
            #endregion

            _content = new TwoSidePanelView(leftPanelState, rightPanelState);
            
            #region LeftPanel
            _localeParamEditor = new LocaleParamEditor("Selected locale", null) {
                style = {
                    minHeight = 100,
                    maxHeight = 150,
                    borderRightWidth = 0,
                    borderLeftWidth = 0,
                    borderTopWidth = 0,
                    borderBottomWidth = 0,
                }
            };

            _localeList = new LocaleList("Locales", null) {
                style = {
                    flexGrow = 1f,
                    borderRightWidth = 0,
                    borderLeftWidth = 0,
                    borderBottomWidth = 0,
                }
            };

            _localeParamEditor.OnChange += LocaleParamEditorOnOnChange;
            _localeList.OnSelect += LocaleSelectEvent;
            _localeList.OnPropertiesClick += LocalePropertiesClickEvent;

            _content.LeftPanel.Add(_localeParamEditor);
            _content.LeftPanel.Add(_localeList);
            _content.LeftPanel.Add(new Button(AddLocaleButtonEvent) {
                text = "Add Locale",
                style = {
                    minHeight = 26,
                    maxWidth = 225,
                    minWidth = 225,
                    alignSelf = new StyleEnum<Align>(Align.Center),
                    marginTop = 10,
                    marginBottom = 10,
                }
            });
            #endregion
            
            _root.Add(_toolbar);
            _root.Add(_content);
            
            var editBox = new TittledContentBox("English (158 terms, 97 notes, 13 empty)") {
                style = {
                    height = new StyleLength(StyleKeyword.Auto),
                    alignSelf = new StyleEnum<Align>(Align.Stretch),
                    flexGrow = 1,
                }
            };

            var functionsBox = new TittledContentBox("Functions") {
                style = {
                    height = new StyleLength(StyleKeyword.Auto),
                    alignSelf = new StyleEnum<Align>(Align.Stretch),
                    flexGrow = 1,
                }
            };

            for (int i = 0; i < 17; i++) editBox.Content.Add(new Button(){text="Term "+i.ToString()});
            for (int i = 0; i < 4; i++) functionsBox.Content.Add(new Button(){text="Function "+i.ToString()});

            _content.RightPanel.Add(functionsBox);
            
            InitializePopups();
            SetupLocaleList();
        }

        private void AddLocaleButtonEvent() {
            Locale locale = LocaleLibraryUtility.AddLocaleToLibrary(SystemLanguage.Unknown);
            if (locale != null) {
                TermStructureInfo[] layout = _localeLibrary.BaseLocale.GetLayout();
#pragma warning disable CS0618
                locale.SetLayout(layout);
#pragma warning restore CS0618
                SetupLocaleList();
            }
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

        private void LocaleParamEditorOnOnChange(LocaleParamEditor self) {
            if (self.Locale != null) {
                LocaleLibraryUtility.RenameLocaleAssetToLanguageMatch(self.Locale);
                UpdateLocaleList();
            }
        }

        private void LocalePropertiesClickEvent(LocaleList self, LocaleListElement selectedElement) {
            _managedLocaleListElement = selectedElement;
            if (_managedLocaleListElement == null || _managedLocaleListElement.Locale == null) return;
            var baseStaticLocale = _localeLibrary.BaseLocale;
            var popup = (_managedLocaleListElement.Locale == baseStaticLocale)
                ? _managedLocaleBaseOptionPopup
                : _managedLocaleOptionPopup; 
            Rect mouseRect = new Rect(Event.current.mousePosition, Vector2.one); 
            UnityEditor.PopupWindow.Show(mouseRect, popup);
        }

        private void LocaleSelectEvent(LocaleList self, LocaleList.SelectionInfo selectionInfo) {
            _localeParamEditor.SetLocale(selectionInfo.Locale);
            //TODO if viewMode == plain, set data to content view
        }

        private void LeftPanelStateChangeEvent(ChangeEvent<bool> evt) {
            leftPanelState = evt.newValue;
            _content.ShowLeftPanel = evt.newValue;
        }
        
        private void RightPanelStateChangeEvent(ChangeEvent<bool> evt) {
            rightPanelState = evt.newValue;
            _content.ShowRightPanel = evt.newValue;
        }

        private void ValidationButtonClickEvent() {
            Rect buttonRect = _validationButton.worldBound;
            buttonRect.width = 0;
            UnityEditor.PopupWindow.Show(buttonRect, _validationOptionPopup);
        }
        
        private void TranslationButtonClickEvent() {
            Rect buttonRect = _translationButton.worldBound;
            buttonRect.width = 0;
            UnityEditor.PopupWindow.Show(buttonRect, _translationOptionPopup);
        }

        private void SetDefaultEditorValues() {
            viewMode = ViewMode.Plain;
            leftPanelState = true;
            rightPanelState = false;
        }
        
        private void SaveEditorValues() {
            EditorPrefs.SetString(EditorSaveKeys.ViewMode, viewMode.ToString());
            EditorPrefs.SetBool(EditorSaveKeys.LeftPanelState, (leftPanelState = _content.ShowLeftPanel));
            EditorPrefs.SetBool(EditorSaveKeys.RightPanelState, (rightPanelState = _content.ShowRightPanel));
        }

        private void LoadEditorValues() {
            SetDefaultEditorValues();
            
            if (EditorPrefs.HasKey(EditorSaveKeys.ViewMode)) {
                string value = EditorPrefs.GetString(EditorSaveKeys.ViewMode);
                Enum.TryParse(value, out viewMode);
            }

            if (EditorPrefs.HasKey(EditorSaveKeys.LeftPanelState)) {
                leftPanelState = EditorPrefs.GetBool(EditorSaveKeys.LeftPanelState);
            }

            if (EditorPrefs.HasKey(EditorSaveKeys.RightPanelState)) {
                rightPanelState = EditorPrefs.GetBool(EditorSaveKeys.RightPanelState);
            }
        }

    }
}