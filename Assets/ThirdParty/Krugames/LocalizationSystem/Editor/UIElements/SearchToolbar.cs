using Krugames.LocalizationSystem.Editor.Styles;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UIElements {
    /// <summary>
    /// Top toolbar with full-width search.
    /// Editor VisualElement for fast interface prototyping.
    /// </summary>
    public class SearchToolbar : Toolbar {

        private readonly ToolbarSearchField _searchField;

        public SearchToolbar() {
            styleSheets.Add(LocalizationEditorStyles.GlobalStyle);
            Add(_searchField = new ToolbarSearchField() {
                style = {
                    minWidth = 0,
                    width = 50,
                    maxWidth = new StyleLength(StyleKeyword.Auto),
                    flexGrow = 1,
                }
            });
        }
    }
}