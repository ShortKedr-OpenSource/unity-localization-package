using Krugames.LocalizationSystem.Editor.Styles;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor {
    public class TittledContentBox : Box {

        protected const string UssClassName = nameof(TittledContentBox);
        
        private Toolbar _toolbar;
        private Label _tittle;
        private VisualElement _content;
        
        public Label Tittle => _tittle;
        public VisualElement Content => _content;

        public TittledContentBox(string tittle) {

            styleSheets.Add(LocalizationEditorStyles.LocalizationEditorStyle);
            
            AddToClassList(UssClassName);
            
            _toolbar = new Toolbar() {
                style = {
                    flexGrow = 1f,
                    minHeight = 21,
                    maxHeight = 21,
                }
            };
            
            _tittle = new Label(tittle) {
                style = {
                    flexGrow = 1f,
                    unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter),
                    unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold),
                }
            };

            _content = new VisualElement() {
                style = {
                    flexGrow = 1f,
                    backgroundColor = new Color(0f, 0f, 0f, 0f),
                }
            };
            
            _toolbar.Add(_tittle);
            
            Add(_toolbar);
            Add(_content);
        }
    }
}