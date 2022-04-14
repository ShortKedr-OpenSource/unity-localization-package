using Krugames.LocalizationSystem.Editor.Styles;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UIElements {
    /// <summary>
    /// Top toolbar with full-width search.
    /// Editor VisualElement for fast interface prototyping.
    /// </summary>
    public class SearchToolbar : Toolbar {

        public delegate void SearchChangeDelegate(string newValue);
        
        private readonly ToolbarSearchField _searchField;

        public event SearchChangeDelegate OnSearchChanged;
        
        public string SearchText {
            get => _searchField.value;
            set => _searchField.value = value;
        }
        
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
            
            _searchField.RegisterCallback<ChangeEvent<string>>(SearchChange_Handler);
        }

        private void SearchChange_Handler(ChangeEvent<string> evt) {
            OnSearchChanged?.Invoke(evt.newValue);
        }
    }
}