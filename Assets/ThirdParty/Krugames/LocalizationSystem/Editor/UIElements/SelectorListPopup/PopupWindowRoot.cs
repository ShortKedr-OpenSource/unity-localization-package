using Krugames.LocalizationSystem.Editor.Styles;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UIElements {
    public class PopupWindowRoot : Box {
        public PopupWindowRoot() {
            styleSheets.Add(LocalizationEditorStyles.GlobalStyle);
        }
    }
}