using System;
using Krugames.LocalizationSystem.Editor.Styles;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI {
    public class LocaleTermEditorToolbar : Toolbar {

        private Label _tittle;
        private ToolbarButton _propertiesButton;

        public LocaleTermEditorToolbar(string tittle, Action propsClickEvent) {
            styleSheets.Add(LocalizationEditorStyles.GlobalStyle);

            _tittle = new Label(tittle) {
                style = {
                    flexGrow = 1,
                    alignContent = new StyleEnum<Align>(Align.FlexEnd),
                    alignItems = new StyleEnum<Align>(Align.Stretch),
                    justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween),
                    flexDirection = new StyleEnum<FlexDirection>(FlexDirection.RowReverse),
                }
            };

            _propertiesButton = new ToolbarButton(propsClickEvent) {
                text = "Properties",
                style = {
                    flexGrow = 0,
                    maxWidth = 100,
                    unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Normal),
                }
            };
            
            _tittle.Add(_propertiesButton);
            Add(_tittle);
        }
    }
}