using Krugames.LocalizationSystem.Editor.Styles;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UIElements {
    /// <summary>
    /// Top toolbar with full-width title.
    /// Editor VisualElement for fast interface prototyping.
    /// </summary>
    public class TittleToolbar : Toolbar {
        
        private Label _titleLabel;

        public string Title {
            get => _titleLabel.text;
            set => _titleLabel.text = value;
        }

        public TittleToolbar(string title) {
            styleSheets.Add(LocalizationEditorStyles.GlobalStyle);
            Add(_titleLabel = new Label(title) {
                style = {
                    flexGrow = 1,
                    width = new StyleLength(StyleKeyword.Auto)
                }
            });
        }
    }
}