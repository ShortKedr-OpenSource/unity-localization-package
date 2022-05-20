using System;
using Krugames.LocalizationSystem.Editor.Package;
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
            Groups = 1,
        }

        private static class EditorSaveKeys {
            public const string ViewMode = "Krugames_LocalizationEditor_ViewMode";
        }

        private static readonly Vector2 MinSize = new Vector2(800, 500);
        private static readonly Vector2 DefaultSize = new Vector2(800, 500);

        
        [SerializeField] private ViewMode viewMode;
        
        
        private VisualElement _root;

        private Toolbar _toolbar; // TODO ElementToolbar, left, right, middle
        private Button _leftPanelToggleButton; //TODO ToggleButton, place to left
        private Button _rightPanelToggleButton; //TODO ToggleButton, place to right
        private EnumField _viewModeSelector; //TODO ButtonEnum, place to center;

        private VisualElement _contentRoot;

        private VisualElement _contentView;
        private VisualElement _leftPanel;
        private VisualElement _rightPanel;
        

        
        [MenuItem("Window/Krugames/Localization")]
        public static void Open() {
            LocalizationEditor editorWindow = EditorWindow.GetWindow<LocalizationEditor>("Localization", true);
            editorWindow.minSize = MinSize;
            editorWindow.position = new Rect(editorWindow.position.position, DefaultSize);
            editorWindow.Show();
        }

        public void CreateGUI() {
            SerializedObject serializedObject = new SerializedObject(this);
            
            ImportStyleSheet();
            
            _root = rootVisualElement;

            _toolbar = new Toolbar() {
                style = {
                    minHeight = 24,
                    maxHeight = 24,
                    flexGrow = 1,
                }
            };

            _leftPanelToggleButton = new ToolbarButton(ToggleLeftPanel) {
                text = "<"
            };

            _rightPanelToggleButton = new ToolbarButton(ToggleRightPanel) {
                text = ">"
            };

            _viewModeSelector = new EnumField("View mode", ViewMode.Plain);
            _viewModeSelector.BindProperty(serializedObject.FindProperty("viewMode"));
            
            _toolbar.Add(_leftPanelToggleButton);
            _toolbar.Add(_viewModeSelector);
            _toolbar.Add(_rightPanelToggleButton);

            _contentRoot = new VisualElement() {
                style = {
                    flexGrow = 1,
                    flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row),
                }
            };
            
            _contentView = new VisualElement() {
                style= {
                    flexGrow = 1,
                    width = new StyleLength(StyleKeyword.Auto),
                    alignItems = new StyleEnum<Align>(Align.Stretch),
                }
            };
            
            _leftPanel = new VisualElement() {
                style= {
                    flexGrow = 0,
                    minWidth = 300,
                    maxWidth = 300,
                    width = new StyleLength(StyleKeyword.Auto),
                    borderRightWidth = 1,
                    borderRightColor = new Color(0, 0, 0, 0.5f), //TODO replace with variable in uss;
                    alignItems = new StyleEnum<Align>(Align.Stretch),
                }
            };
            
            _rightPanel = new VisualElement() {
                style= {
                    flexGrow = 0,
                    minWidth = 300,
                    maxWidth = 300,
                    width = new StyleLength(StyleKeyword.Auto),
                    borderLeftWidth = 1,
                    borderLeftColor = new Color(0, 0, 0, 0.5f), //TODO replace with variable in uss;
                    alignItems = new StyleEnum<Align>(Align.Stretch),
                }
            };
            
            _contentRoot.Add(_contentView);
            ToggleLeftPanel();

            _root.Add(_toolbar);
            _root.Add(_contentRoot);

            var localesBox = new TittledContentBox("Locales") {
                style = {
                    height = new StyleLength(StyleKeyword.Auto),
                    alignSelf = new StyleEnum<Align>(Align.Stretch),
                    flexGrow = 1,
                }
            };

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

            for (int i = 0; i < 3; i++) localesBox.Content.Add(new Button(){text="Locale "+i.ToString()});
            for (int i = 0; i < 17; i++) editBox.Content.Add(new Button(){text="Term "+i.ToString()});
            for (int i = 0; i < 4; i++) functionsBox.Content.Add(new Button(){text="Function "+i.ToString()});

            _leftPanel.Add(localesBox);
            _contentView.Add(editBox);
            _rightPanel.Add(functionsBox);
        }

        private void Awake() {
            LoadEditorValues();
        }
        
        private void OnDestroy() {
            SaveEditorValues();
        }

        private void ImportStyleSheet() {
            string ussPath = PackageVariables.PackagePath + "Editor/UI/LocalizationEditor/LocalizationEditor.uss";
            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(ussPath);
            if (styleSheet != null) rootVisualElement.styleSheets.Add(styleSheet);
            else Debug.LogError($"{nameof(LocalizationEditor)} uss style sheet asset not found!");
        }

        private void SetLeftPanelState(bool state) {
            if (state) {
                _contentRoot.Add(_leftPanel);
                _leftPanel.PlaceBehind(_contentView);
            } else {
                _leftPanel.RemoveFromHierarchy();
            }
        }

        private void SetRightPanelState(bool state) {
            if (state) {
                _contentRoot.Add(_rightPanel);
                _rightPanel.PlaceInFront(_contentView);
            } else {
                _rightPanel.RemoveFromHierarchy();
            }
        }
        
        private void ToggleLeftPanel() {
            SetLeftPanelState(!GetLeftPanelState());
        }

        private void ToggleRightPanel() {
            SetRightPanelState(!GetRightPanelState());
        }

        private bool GetLeftPanelState() {
            return _contentRoot.Contains(_leftPanel);
        }

        private bool GetRightPanelState() {
            return _contentRoot.Contains(_rightPanel);
        }

        private void SetDefaultEditorValues() {
            viewMode = ViewMode.Plain;
        }
        
        private void SaveEditorValues() {
            EditorPrefs.SetString(EditorSaveKeys.ViewMode, viewMode.ToString());
        }

        private void LoadEditorValues() {
            SetDefaultEditorValues();
            
            if (EditorPrefs.HasKey(EditorSaveKeys.ViewMode)) {
                string value = EditorPrefs.GetString(EditorSaveKeys.ViewMode);
                Enum.TryParse(value, out viewMode);
            }
        }

    }
}