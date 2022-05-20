using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor {
    public class ElementToolbar : Toolbar {

        private const string LeftAnchorClassName = nameof(ElementToolbar) + "_LeftAnchor";
        private const string RightAnchorClassName = nameof(ElementToolbar) + "_RightAnchor";
        private const string CenterAnchorClassName = nameof(ElementToolbar) + "_CenterAnchor";
        
        private VisualElement _leftAnchor;
        private VisualElement _rightAnchor;
        private VisualElement _centerAnchor;

        public VisualElement LeftAnchor => _leftAnchor;
        public VisualElement RightAnchor => _rightAnchor;
        public VisualElement CenterAnchor => _centerAnchor;

        public ElementToolbar() {

            _leftAnchor = new VisualElement() {
                pickingMode = PickingMode.Ignore,
                focusable = false,
            };
            _rightAnchor = new VisualElement() {
                pickingMode = PickingMode.Ignore,
                focusable = false,
            };
            _centerAnchor = new VisualElement() {
                pickingMode = PickingMode.Ignore,
                focusable = false,
            };
            
            _leftAnchor.AddToClassList(LeftAnchorClassName);
            _rightAnchor.AddToClassList(RightAnchorClassName);
            _centerAnchor.AddToClassList(CenterAnchorClassName);
            
            Add(_leftAnchor);
            Add(_rightAnchor);
            Add(_centerAnchor);
        }
    }
}