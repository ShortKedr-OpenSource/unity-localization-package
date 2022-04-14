using Krugames.LocalizationSystem.Editor.Package;
using Krugames.LocalizationSystem.Editor.Styles;
using Krugames.LocalizationSystem.Editor.UIElements;
using Krugames.LocalizationSystem.Models.Locators;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ThirdParty.Krugames.LocalizationSystem.Model.Editor {
    public class LocaleTermSelector : PopupWindowContent {
        
        private static LocaleTermLocator.LocaleTermBuildData[] _buildData = LocaleTermLocator.BuildData;

        private PopupWindowRoot _popupRoot;
        
        private SearchToolbar _searchToolbar;
        private TittleToolbar _tittleToolbar;
        private ListGroup _listGroup;

        public override void OnOpen() {

            VisualElement root = editorWindow.rootVisualElement;
            root.styleSheets.Add(LocalizationEditorStyles.GlobalStyle);
            root.style.marginTop = 0;
            root.style.marginBottom = 0;
            root.style.marginLeft = 0;
            root.style.marginRight = 0;
            
            root.Add(_popupRoot = new PopupWindowRoot());

            _popupRoot.Add(_searchToolbar = new SearchToolbar());
            _popupRoot.Add(_tittleToolbar = new TittleToolbar("Locale terms"));
            _popupRoot.Add(_listGroup = new ListGroup());

            
            for (int i = 0; i < _buildData.Length; i++) {
                _listGroup.Add(new ListSelectableElement() {text = _buildData[i].Name});
            }
        }
        
        public override Vector2 GetWindowSize() {
            return new Vector2(225, 300);
        }

        public override void OnGUI(Rect rect) {
            //do nothing since we implement it with UIElements
        }
    }
}