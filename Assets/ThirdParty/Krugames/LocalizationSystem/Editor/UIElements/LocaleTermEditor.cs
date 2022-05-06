using Krugames.LocalizationSystem.Models;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UIElements {
    public class LocaleTermEditor : Box {

        //TODO add vert scrollview
        //TODO add properties button
        
        private LocaleTerm _term;
        private IMGUIContainer _editorContainer;

        private UnityEditor.Editor _termEditor;

        public LocaleTerm Term => _term;

        public LocaleTermEditor() {

            style.width = new StyleLength(StyleKeyword.Auto);

            style.borderTopLeftRadius = 5f;
            style.borderTopRightRadius = 5f;
            style.borderBottomLeftRadius = 5f;
            style.borderBottomRightRadius = 5f;

            style.marginTop = 4f;
            style.marginBottom = 4f;
            style.marginRight = 4f;
            
            style.minHeight = 125f;

            style.borderTopColor = new Color(0.0f,0.0f,0.0f, 0.25f);
            style.borderBottomColor = new Color(0.0f,0.0f,0.0f, 0.25f);
            style.borderLeftColor = new Color(0.0f,0.0f,0.0f, 0.25f);
            style.borderRightColor = new Color(0.0f,0.0f,0.0f, 0.25f);
                
            style.borderTopWidth = 1f;
            style.borderBottomWidth = 1f;
            style.borderLeftWidth = 1f;
            style.borderRightWidth = 1f;

            var toolbar = new Toolbar() {
                style = {
                    borderTopLeftRadius = 5f,
                    borderTopRightRadius = 5f,
                    alignContent = new StyleEnum<Align>(Align.Center),
                    justifyContent = new StyleEnum<Justify>(Justify.Center),
                    unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter),
                    unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold),
                }
            };
            toolbar.Add(new Label("Selected term"));

            Add(toolbar);
            Add(_editorContainer = new IMGUIContainer(OnTermEditorGUI) {
                style = {
                    flexGrow = 1f,
                    width = new StyleLength(StyleKeyword.Auto),
                    marginTop = 5f,
                    marginBottom = 5f,
                    marginLeft = 5f,
                    marginRight = 5f,
                }
            });
        }

        public void SetTerm(LocaleTerm term) {
            _term = term;
            if (_termEditor != null) Object.DestroyImmediate(_termEditor);
            if (_term != null) _termEditor = UnityEditor.Editor.CreateEditor(_term);
        }

        private void OnTermEditorGUI() {
            if (_termEditor != null) _termEditor.OnInspectorGUI();
        }
    }
}