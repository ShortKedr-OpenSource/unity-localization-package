using System.Runtime.CompilerServices;
using Krugames.LocalizationSystem.Editor.Styles;
using Krugames.LocalizationSystem.Models;
using UnityEditor;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UIElements {
    public class LocaleTermListViewElement : Box {

        private const string EvenClassName = "Even";
        private const string OddClassName = "Odd";
        private const string TermLabelClassName = nameof(LocaleTermListViewElement)+"_TermLabel";
        private const string ValueLabelClassName = nameof(LocaleTermListViewElement)+"_ValueLabel";
        
        private LocaleTerm _localeTerm;
        private FillRule _fillRule;

        private readonly Label _termLabel;
        private readonly Label _valueLabel; 

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
                style = {
                    minWidth = EditorGUIUtility.labelWidth,
                }
            };

            _valueLabel = new Label();

            _termLabel.AddToClassList(TermLabelClassName);
            _valueLabel.AddToClassList(ValueLabelClassName);

            Add(_termLabel);
            Add(_valueLabel);
            
            SetTerm(localeTerm);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetTerm(LocaleTerm localeTerm) {
            _localeTerm = localeTerm;
            if (_localeTerm == null) {
                visible = false;
                return;
            }
            visible = true;
            _termLabel.text = _localeTerm.Term;
            _valueLabel.text = _localeTerm.Value.ToString();
        }

        private void AddFillRuleClass() {
            if (_fillRule == FillRule.Even) AddToClassList(EvenClassName);
            else if (_fillRule == FillRule.Odd) AddToClassList(OddClassName);
        }

        private void RemoveFillRuleClass() {
            if (_fillRule == FillRule.Even) RemoveFromClassList(EvenClassName);
            else if (_fillRule == FillRule.Odd) RemoveFromClassList(OddClassName);
        }
        
        
    }
}