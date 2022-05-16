using Krugames.LocalizationSystem.Common.Editor.UnityInternal;
using Krugames.LocalizationSystem.Editor.Styles;
using Krugames.LocalizationSystem.Models;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI {
    public class LocaleTermEditor : Box {
        
        private const string RootClassName = nameof(LocaleTermEditor) + "_Root";
        
        private LocaleTerm _term;

        private VisualElement _root;
        
        private LocaleTermEditorToolbar _toolbar;
        private VisualElement _content;
        private ScrollView _scrollView;
        private IMGUIContainer _editorContainer;

        private UnityEditor.Editor _termEditor;
        private bool _allowDrawing = false;

        public LocaleTerm Term => _term;

        public LocaleTermEditor() : this(null) {
        }

        public LocaleTermEditor(LocaleTerm term) {
            styleSheets.Add(LocalizationEditorStyles.GlobalStyle);
            
            _root = new VisualElement();
            _root.AddToClassList(RootClassName);
            Add(_root);
            
            _toolbar = new LocaleTermEditorToolbar("SelectedTerm", PropsClickEvent);

            _scrollView = new ScrollView(ScrollViewMode.Vertical) {
                style = {
                    flexGrow = 1.0f,
                    width = new StyleLength(StyleKeyword.Auto),
                    paddingTop = 5f,
                    paddingLeft = 5f,
                    paddingRight = 5f,
                }
            };

            _editorContainer = new IMGUIContainer(OnTermEditorGUI) {
                style = {
                    flexGrow = 1f,
                    width = new StyleLength(StyleKeyword.Auto),
                }
            };
            
            _scrollView.Add(_editorContainer);
            _root.Add(_toolbar);
            _root.Add(_scrollView);
            
            SetTerm(term);
        }

        private void PropsClickEvent() {
            if (_term == null) return;
            EditorInternalUtility.OpenPropertyEditor(_term);
        }

        public void SetTerm(LocaleTerm term) {
            _term = term;
            if (_termEditor != null) Object.DestroyImmediate(_termEditor);
            if (_term != null) _termEditor = UnityEditor.Editor.CreateEditor(_term);
            _allowDrawing = _termEditor != null;
        }

        private void OnTermEditorGUI() {
            if (!_allowDrawing) return;
            _termEditor.OnInspectorGUI();
        }
    }
}