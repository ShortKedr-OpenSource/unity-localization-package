using System;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor {
    public class ToggleButton : Button {
        public ToggleButton() {
        }

        public ToggleButton(Action clickEvent) : base(clickEvent) {
        }
    }
}