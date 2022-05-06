using Krugames.LocalizationSystem.Editor.Styles;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UIElements {
    public class LocaleTermListViewContent : VisualElement {
        public LocaleTermListViewContent() {
            styleSheets.Add(LocalizationEditorStyles.GlobalStyle);
        }
    }
}