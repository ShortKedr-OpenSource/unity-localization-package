using Krugames.LocalizationSystem.Editor.Styles;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UIElements {
    /// <summary>
    /// Group of list elements. Similar to "Add Component" menu list.
    /// Editor VisualElement for fast interface prototyping.
    /// </summary>
    public class ListGroup : ScrollView {
        public ListGroup() : this(ScrollViewMode.Vertical) {
        }

        public ListGroup(ScrollViewMode scrollViewMode) : base(scrollViewMode) {
            styleSheets.Add(LocalizationEditorStyles.GlobalStyle);
        }
    }
}