using System.Runtime.CompilerServices;
using Krugames.LocalizationSystem.Editor.Styles;
using Krugames.LocalizationSystem.Implementation;
using Krugames.LocalizationSystem.Models;
using ThirdParty.Krugames.LocalizationSystem.Editor.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using PopupWindow = UnityEditor.PopupWindow;

namespace Krugames.LocalizationSystem.Editor.UI {
    public class LocaleTermListViewElement : Box {

        private const string EvenClassName = "Even";
        private const string OddClassName = "Odd";
        private const string TermLabelClassName = nameof(LocaleTermListViewElement)+"_TermLabel";
        private const string ValueLabelClassName = nameof(LocaleTermListViewElement)+"_ValueLabel";
        private const string PropsButtonClassName = nameof(LocaleTermListViewElement)+"_PropsButton";
        
        private LocaleTerm _localeTerm;
        private FillRule _fillRule;

        private readonly Label _termLabel;
        private readonly Label _valueLabel;
        private readonly Button _propsButton;

        private readonly Clickable _clickable;

        
        public delegate void ClickDelegate(LocaleTermListViewElement element);
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

        
        public LocaleTermListViewElement(LocaleTerm localeTerm, FillRule fillRule) {
            styleSheets.Add(LocalizationEditorStyles.GlobalStyle);
            _fillRule = fillRule;
            AddFillRuleClass();

            _termLabel = new Label() {
                pickingMode = PickingMode.Ignore,
                style = {
                    minWidth = EditorGUIUtility.labelWidth,
                }
            };

            _valueLabel = new Label() {
                pickingMode = PickingMode.Ignore,
            };

            _propsButton = new Button(PropertiesButtonClickEvent); 

            _termLabel.AddToClassList(TermLabelClassName);
            _valueLabel.AddToClassList(ValueLabelClassName);
            _propsButton.AddToClassList(PropsButtonClassName);
            
            Add(_termLabel);
            Add(_valueLabel);
            Add(_propsButton);

            _clickable = new Clickable(ElementClickEvent);
            this.AddManipulator(_clickable);

            RegisterCallback<MouseUpEvent>(ElementMouseUpEvent);
            
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