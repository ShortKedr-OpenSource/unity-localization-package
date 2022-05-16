using Krugames.LocalizationSystem.Editor.Styles;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI {
    public class PopupWindowRoot : Box {
        public PopupWindowRoot() {
            styleSheets.Add(LocalizationEditorStyles.GlobalStyle);
        }
    }
}