using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor {
    /// <summary>
    /// Visual Element with panel on each side
    /// </summary>
    public class TwoSidePanelView : VisualElement {

        private const string ContentClassName = nameof(TwoSidePanelView) + "_Content";
        private const string LeftPanelClassName = nameof(TwoSidePanelView) + "_LeftPanel";
        private const string RightPanelClassName = nameof(TwoSidePanelView) + "_RightPanel";
        
        private VisualElement _content;
        private VisualElement _leftPanel;
        private VisualElement _rightPanel;

        private bool _showLeftPanel;
        private bool _showRightPanel;

        
        public VisualElement Content => _content;

        public VisualElement LeftPanel => _leftPanel;

        public VisualElement RightPanel => _rightPanel;

        
        public bool ShowLeftPanel {
            get => _showLeftPanel;
            set {
                _showLeftPanel = value;
                SetLeftPanelState(_showLeftPanel);
            }
        }

        public bool ShowRightPanel {
            get => _showRightPanel;
            set {
                _showRightPanel = value;
                SetRightPanelState(_showRightPanel);
            }
        }

        public TwoSidePanelView(bool showLeftPanel, bool showRightPanel) {
            
            _showLeftPanel = showLeftPanel;
            _showRightPanel = showRightPanel;

            _content = new VisualElement();
            _leftPanel = new VisualElement();
            _rightPanel = new VisualElement();
            
            _content.AddToClassList(ContentClassName);
            _leftPanel.AddToClassList(LeftPanelClassName);
            _rightPanel.AddToClassList(RightPanelClassName);

            Add(_content);
            
            SetLeftPanelState(_showLeftPanel);
            SetRightPanelState(_showRightPanel);
        }

        private void SetLeftPanelState(bool state) {
            if (state) {
                Add(_leftPanel);
                _leftPanel.PlaceBehind(_content);
            } else {
                _leftPanel.RemoveFromHierarchy();
            }
        }
        
        private void SetRightPanelState(bool state) {
            if (state) {
                Add(_rightPanel);
                _rightPanel.PlaceInFront(_content);
            } else {
                _rightPanel.RemoveFromHierarchy();
            }
        }
        
        public void ToggleLeftPanel() {
            ShowLeftPanel = !ShowLeftPanel;
        }

        public void ToggleRightPanel() {
            ShowRightPanel = !ShowRightPanel;
        }
    }
}