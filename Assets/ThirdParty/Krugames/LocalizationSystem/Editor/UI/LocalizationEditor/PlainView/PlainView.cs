using Krugames.LocalizationSystem.Editor.Styles;
using Krugames.LocalizationSystem.Models;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor.PlainView {
    public class PlainView : VisualElement {

        private Locale _locale;

        public PlainView(Locale locale) {
            _locale = locale;
            
            styleSheets.Add(LocalizationEditorStyles.LocalizationEditorStyle);
        }

        //TODO private methods
        
        public void SetLocale(Locale locale) {
            
        }
    }
}