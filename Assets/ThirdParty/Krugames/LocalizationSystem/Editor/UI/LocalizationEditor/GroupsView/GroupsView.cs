using Krugames.LocalizationSystem.Editor.Styles;
using Krugames.LocalizationSystem.Models;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor.GroupsView {
    public class GroupsView : VisualElement {

        private LocaleLibrary _localeLibrary;

        public GroupsView(LocaleLibrary localeLibrary) {
            styleSheets.Add(LocalizationEditorStyles.LocalizationEditorStyle);
        }

        //TODO private 
        
        public void SetLibrary(LocaleLibrary library) {
            
        }
    }
}