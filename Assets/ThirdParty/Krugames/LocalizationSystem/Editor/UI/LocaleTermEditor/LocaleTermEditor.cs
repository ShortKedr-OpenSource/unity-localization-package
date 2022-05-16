using Krugames.LocalizationSystem.Models;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI {
    //TODO move to USS
    public class LocaleTermEditor : Box {

        //TODO add vert scrollview
        //TODO add properties button
        //TODO add overflow mechanism
        //TODO add term name
        
        private LocaleTerm _term;

        private Toolbar _toolbar;
        private VisualElement _content;
        private ScrollView _scrollView;
        private IMGUIContainer _editorContainer;

        private UnityEditor.Editor _termEditor;
        private bool _allowDrawing = false;

        public delegate void EventDelegate(LocaleTermEditor self);
        public event EventDelegate OnChange;
        
        
        public LocaleTerm Term => _term;

        public LocaleTermEditor() : this(null) {
        }

        public LocaleTermEditor(LocaleTerm term) {

            style.width = new StyleLength(StyleKeyword.Auto);
            style.height = new StyleLength(StyleKeyword.Auto);

            style.borderTopLeftRadius = 5f;
            style.borderTopRightRadius = 5f;
            style.borderBottomLeftRadius = 5f;
            style.borderBottomRightRadius = 5f;

            style.marginTop = 4f;
            style.marginBottom = 4f;
            style.marginRight = 4f;

            style.minHeight = 125f;
            style.maxHeight = 125f;
            style.overflow = new StyleEnum<Overflow>(Overflow.Hidden);

            style.borderTopColor = new Color(0.0f,0.0f,0.0f, 0.25f);
            style.borderBottomColor = new Color(0.0f,0.0f,0.0f, 0.25f);
            style.borderLeftColor = new Color(0.0f,0.0f,0.0f, 0.25f);
            style.borderRightColor = new Color(0.0f,0.0f,0.0f, 0.25f);
                
            style.borderTopWidth = 1f;
            style.borderBottomWidth = 1f;
            style.borderLeftWidth = 1f;
            style.borderRightWidth = 1f;

            _toolbar = new Toolbar() {
                style = {
                    borderTopLeftRadius = 5f,
                    borderTopRightRadius = 5f,
                    alignContent = new StyleEnum<Align>(Align.Center),
                    justifyContent = new StyleEnum<Justify>(Justify.Center),
                    unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter),
                    unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold),
                    minHeight = 24,
                    maxHeight = 24,
                }
            };

            var tittle = new Label("Selected term") {
                style = {
                    flexGrow = 1,
                    flexDirection = new StyleEnum<FlexDirection>(FlexDirection.RowReverse),
                    alignContent = new StyleEnum<Align>(Align.FlexEnd),
                    alignItems = new StyleEnum<Align>(Align.Stretch),
                }
            };
            tittle.Add(new ToolbarButton(() => UnityObjectPropertiesEditor.Create(_term)) {
                text = "Properties",
                style = {
                    flexGrow = 0,
                    maxWidth = 100,
                    unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Normal),
                }
            });
            _toolbar.Add(tittle);

            Add(_toolbar);
            Add(_scrollView = new ScrollView(ScrollViewMode.Vertical) {
                style = {
                    flexGrow = 1.0f,
                    width = new StyleLength(StyleKeyword.Auto),
                    paddingTop = 5f,
                    paddingLeft = 5f,
                    paddingRight = 5f,
                }
            });
            _scrollView.Add(_editorContainer = new IMGUIContainer(OnTermEditorGUI) {
                style = {
                    flexGrow = 1f,
                    width = new StyleLength(StyleKeyword.Auto),
                }
            });
            
            SetTerm(term);
        }

        public void SetTerm(LocaleTerm term) {
            _term = term;
            if (_termEditor != null) Object.DestroyImmediate(_termEditor);
            if (_term != null) _termEditor = UnityEditor.Editor.CreateEditor(_term);
            _allowDrawing = _termEditor != null;
        }

        private void OnTermEditorGUI() {
            if (!_allowDrawing) return;
            TermEditorGUI();
        }

        private void TermEditorGUI() {
            _termEditor.OnInspectorGUI();

            if (EditorGUI.EndChangeCheck()) {
                OnChange?.Invoke(this);
            }
        }
    }
}