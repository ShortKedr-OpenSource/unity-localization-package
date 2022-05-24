using Krugames.LocalizationSystem.Editor.Styles;
using UnityEngine;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor {
    /// <summary>
    /// Visual Element with panel on each side
    /// </summary>
    public class TwoSidePanelView : VisualElement {

        private const string ContentClassName = nameof(TwoSidePanelView) + "_Content";
        private const string LeftPanelClassName = nameof(TwoSidePanelView) + "_LeftPanel";
        private const string RightPanelClassName = nameof(TwoSidePanelView) + "RightPanel";
        
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
            
            styleSheets.Add(LocalizationEditorStyles.LocalizationEditorStyle);
            
            _showLeftPanel = showLeftPanel;
            _showRightPanel = showRightPanel;

            style.flexGrow = 1;
            style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            
            _content = new VisualElement() {
                style = {
                    flexGrow = 1,
                    width = new StyleLength(StyleKeyword.Auto),
                    alignItems = new StyleEnum<Align>(Align.Stretch),
                }
            };

            _leftPanel = new VisualElement() {
                style= {
                    flexGrow = 0,
                    minWidth = 300,
                    maxWidth = 300,
                    width = new StyleLength(StyleKeyword.Auto),
                    borderRightWidth = 1,
                    borderLeftWidth = 1,
                    borderBottomWidth = 1,
                    borderRightColor = new Color(0, 0, 0, 0.5f), //TODO replace with variable in uss;
                    borderLeftColor = new Color(0, 0, 0, 0.5f), //TODO replace with variable in uss;
                    borderBottomColor = new Color(0, 0, 0, 0.5f), //TODO replace with variable in uss;
                    alignItems = new StyleEnum<Align>(Align.Stretch),
                    overflow = new StyleEnum<Overflow>(Overflow.Hidden),
                }
            };

            _rightPanel = new VisualElement() {
                style = {
                    flexGrow = 0,
                    minWidth = 300,
                    maxWidth = 300,
                    width = new StyleLength(StyleKeyword.Auto),
                    borderRightWidth = 1,
                    borderLeftWidth = 1,
                    borderBottomWidth = 1,
                    borderRightColor = new Color(0, 0, 0, 0.5f), //TODO replace with variable in uss;
                    borderLeftColor = new Color(0, 0, 0, 0.5f), //TODO replace with variable in uss;
                    borderBottomColor = new Color(0, 0, 0, 0.5f), //TODO replace with variable in uss;
                    alignItems = new StyleEnum<Align>(Align.Stretch),
                    overflow = new StyleEnum<Overflow>(Overflow.Hidden),
                }
            };

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