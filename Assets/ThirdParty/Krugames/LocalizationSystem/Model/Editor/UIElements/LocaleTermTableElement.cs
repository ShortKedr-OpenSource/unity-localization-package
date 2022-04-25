using Krugames.LocalizationSystem.Editor.Styles;
using Krugames.LocalizationSystem.Models;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace ThirdParty.Krugames.LocalizationSystem.Model.Editor.UIElements {
    public class LocaleTermTableElement : Box {
        private LocaleTerm _localeTerm;

        private Label _termLabel;
        private Label _valueLabel;

        public LocaleTermTableElement(LocaleTerm localeTerm) {
            styleSheets.Add(LocalizationEditorStyles.GlobalStyle);

            _localeTerm = localeTerm; 
            
            Add(_termLabel = new Label(_localeTerm.Term) {
                style = {
                    flexGrow = 0f,
                    borderRightWidth = 1f,
                    borderRightColor = Color.black,
                    maxWidth = 140,
                    minWidth = 140,
                    marginRight = 10,
                    maxHeight = 20,
                }
            });

            Add(_valueLabel = new Label(_localeTerm.Value.ToString()) {
                style = {
                    flexGrow = 1f,
                    maxHeight = 20,
                    overflow = new StyleEnum<Overflow>(),
                }
            });
        }
        
        
    }
}