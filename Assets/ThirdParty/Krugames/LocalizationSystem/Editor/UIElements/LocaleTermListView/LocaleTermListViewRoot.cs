using Krugames.LocalizationSystem.Editor.Styles;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UIElements {
    public class LocaleTermListViewRoot : VisualElement {
        public LocaleTermListViewRoot() {
            styleSheets.Add(LocalizationEditorStyles.GlobalStyle);
        }
    }
}