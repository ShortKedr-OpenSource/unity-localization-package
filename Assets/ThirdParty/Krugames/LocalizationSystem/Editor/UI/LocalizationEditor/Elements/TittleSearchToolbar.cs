using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor {
    public class TittleSearchToolbar : Toolbar {

        private const string TittleClassName = nameof(TittleSearchToolbar) + "_Tittle";
        private const string SearchClassName = nameof(TittleSearchToolbar) + "_Search";

        private Label _tittleLabel;
        private ToolbarSearchField _searchField;

        public Label TittleLabel => _tittleLabel;
        public ToolbarSearchField SearchField => _searchField;

        public TittleSearchToolbar(string tittle) {
            //TODO review, style imports many times, cuz of hierarchy
            //styleSheets.Add(LocalizationEditorStyles.LocalizationEditorStyle);

            _tittleLabel = new Label(tittle);

            _searchField = new ToolbarSearchField();

            _tittleLabel.AddToClassList(TittleClassName);
            _searchField.AddToClassList(SearchClassName);

            _tittleLabel.Add(_searchField);
            Add(_tittleLabel);
        }
    }
}