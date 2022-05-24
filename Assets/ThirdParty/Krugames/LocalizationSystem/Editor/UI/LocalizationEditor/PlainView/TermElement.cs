using System.Runtime.CompilerServices;
using Codice.Client.BaseCommands.Import;
using Krugames.LocalizationSystem.Editor.Styles;
using Krugames.LocalizationSystem.Models;
using UnityEditor;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor.PlainView {
    public class TermElement : Box {

        private const string ActiveClassName = "Active";
        private const string EvenClassName = "Even";
        private const string OddClassName = "Odd";
        private const string MainRootClassName = nameof(TermElement) + "_MainRoot";
        private const string EditorRootClassName = nameof(TermElement) + "_EditorRoot";
        private const string TermLabelClassName = nameof(TermElement)+"_TermLabel";
        private const string ValueLabelClassName = nameof(TermElement)+"_ValueLabel";
        private const string NoteLabelClassName = nameof(TermElement) + "_NoteLabel";
        private const string PropsButtonClassName = nameof(TermElement)+"_PropsButton";
        
        private LocaleTerm _localeTerm;
        private FillRule _fillRule;

        private readonly Box _mainRoot;
        private readonly Box _editorRoot;
        
        private readonly Label _termLabel;
        private readonly Label _valueLabel;
        private readonly Label _noteLabel;
        private readonly Button _propsButton;

        private readonly Clickable _clickable;

        
        public delegate void ClickDelegate(TermElement element);
        public event ClickDelegate OnClick;
        public event ClickDelegate OnPropertiesClick;

        
        public LocaleTerm LocaleTerm => _localeTerm;

        public Label TermLabel => _termLabel;

        public Label ValueLabel => _valueLabel;

        public FillRule FillRule {
            get => _fillRule;
            set {
                RemoveFillRuleClass();
                _fillRule = value;
                AddFillRuleClass();
            }
        }

        
        public TermElement(LocaleTerm localeTerm, FillRule fillRule) {
            //TODO review, style imports many times, cuz of hierarchy
            //styleSheets.Add(LocalizationEditorStyles.LocalizationEditorStyle);
            
            _fillRule = fillRule;
            AddFillRuleClass();

            _termLabel = new Label() {
                pickingMode = PickingMode.Position,
            };

            _valueLabel = new Label() {
                pickingMode = PickingMode.Position,
            };

            string shit =
                "What a fuck is this? How does it work? I don't know. Learn more about the locale library, a collection of useful and interesting functions.";
            _noteLabel = new Label(shit) {
                pickingMode = PickingMode.Position,
                tooltip = shit
            };

            _propsButton = new Button(PropertiesButtonClickEvent); 

            _termLabel.AddToClassList(TermLabelClassName);
            _valueLabel.AddToClassList(ValueLabelClassName);
            _noteLabel.AddToClassList(NoteLabelClassName);
            _propsButton.AddToClassList(PropsButtonClassName);
            
            Add(_termLabel);
            Add(_valueLabel);
            Add(_noteLabel);
            Add(_propsButton);

            _clickable = new Clickable(ElementClickEvent);
            this.AddManipulator(_clickable);

            RegisterCallback<MouseUpEvent>(ElementMouseUpEvent);
            _noteLabel.RegisterCallback<ChangeEvent<string>>(Event_NoteChange);
            
            SetTerm(localeTerm);
        }

        private void AddFillRuleClass() {
            if (_fillRule == FillRule.Even) AddToClassList(EvenClassName);
            else if (_fillRule == FillRule.Odd) AddToClassList(OddClassName);
        }

        private void RemoveFillRuleClass() {
            if (_fillRule == FillRule.Even) RemoveFromClassList(EvenClassName);
            else if (_fillRule == FillRule.Odd) RemoveFromClassList(OddClassName);
        }

        private void ElementClickEvent() {
            OnClick?.Invoke(this);
        }

        private void PropertiesButtonClickEvent() {
            OnPropertiesClick?.Invoke(this);
        }
        
        private void ElementMouseUpEvent(MouseUpEvent evt) {
            if (evt.button == 1) OnPropertiesClick?.Invoke(this);
        }
        
        private void Event_NoteChange(ChangeEvent<string> evt) {
            _noteLabel.tooltip = evt.newValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetTerm(LocaleTerm localeTerm) {
            _localeTerm = localeTerm;
            Update();
        }

        public void Update() {
            if (_localeTerm == null) {
                visible = false;
                return;
            }
            visible = true;
            _termLabel.text = _localeTerm.Term;
            _valueLabel.text = _localeTerm.Value.ToString();
        }

    }
}