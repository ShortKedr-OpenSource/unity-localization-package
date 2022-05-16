using Krugames.LocalizationSystem.Implementation;
using UnityEngine;

namespace ThirdParty.Krugames.LocalizationSystem.Editor.UI {
    public class OptionPopup : SelectorListPopup {

        private int _elementCount;
        private float _popupWidth;
        
        public OptionPopup(Element[] elements, float popupWidth = 200f) : base(null, elements, false, false) {
            _elementCount = elements.Length;
            _popupWidth = popupWidth;
        }

        public override Vector2 GetWindowSize() {
            return new Vector2(_popupWidth, _elementCount * 24f + 2f);
        }
    }
}