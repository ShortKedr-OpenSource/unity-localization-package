using System;
using Krugames.LocalizationSystem.Editor.Styles;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UIElements {
    /// <summary>
    /// Selectable text element of  ListGroup.
    /// Editor VisualElement for fast interface prototyping.
    /// </summary>
    public class ListSelectableElement : Button {
        public ListSelectableElement() : this(null){
        }

        public ListSelectableElement(Action clickEvent) : base(clickEvent) {
            styleSheets.Add(LocalizationEditorStyles.GlobalStyle);
        }
    }
}