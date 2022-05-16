using Krugames.LocalizationSystem.Editor.Styles;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI {
    public class TittleSearchToolbar : Toolbar {

        private const string TittleClassName = nameof(TittleSearchToolbar) + "_Tittle";
        private const string SearchClassName = nameof(TittleSearchToolbar) + "_Search";
        
        private Label _tittleLabel;
        private ToolbarSearchField _searchField;

        public Label TittleLabel => _tittleLabel;
        public ToolbarSearchField SearchField => _searchField;

        public TittleSearchToolbar(string tittle) {
            styleSheets.Add(LocalizationEditorStyles.GlobalStyle);

            _tittleLabel = new Label(tittle);

            _searchField = new ToolbarSearchField();
            
            _tittleLabel.AddToClassList(TittleClassName);
            _searchField.AddToClassList(SearchClassName);
            
            _tittleLabel.Add(_searchField);
            Add(_tittleLabel);
            
            /*var termsLabel = new Label("Terms") {
                style = {
                    flexGrow = 1f,
                    unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter),
                    flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row),
                    justifyContent = new StyleEnum<Justify>(Justify.FlexEnd),
                    unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold),
                }
            };

            var search = new ToolbarSearchField() {
                style = {
                    maxWidth = 100f,
                    width = 100f,
                    minWidth = 10f,
                }
            };*/
        }
    }
}