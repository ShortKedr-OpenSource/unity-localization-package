using Krugames.LocalizationSystem.Editor.Styles;
using Krugames.LocalizationSystem.Models;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor {
    public class GroupsView : VisualElement {

        private LocaleLibrary _localeLibrary;

        public GroupsView(LocaleLibrary localeLibrary) {
            //TODO review, style imports many times, cuz of hierarchy
            //styleSheets.Add(LocalizationEditorStyles.LocalizationEditorStyle);
            
            Add(new Label("Groups view"));
        }

        //TODO private 
        
        public void SetLibrary(LocaleLibrary library) {
            
        }
    }
}