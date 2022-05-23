using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor {
    public class EnumButton<TEnumType> : BindableElement, INotifyValueChanged<int> where TEnumType : Enum {

        private static readonly Type EnumType = typeof(TEnumType);
        
        private int _value;
        private readonly Array _enumValues;
        
        private ToggleButton[] _buttons;
        private Dictionary<ToggleButton, int> _indexByToggleButton;

        public EnumButton(TEnumType value) {
            _enumValues = Enum.GetValues(EnumType);
            _buttons = new ToggleButton[_enumValues.Length];
            _indexByToggleButton = new Dictionary<ToggleButton, int>(_enumValues.Length);
            for (int i = 0; i < _enumValues.Length; i++) {
                _buttons[i] = new ToggleButton(_value == i, ToggleButtonChangeEvent, ToggleButton.ToggleMode.Pickable) {
                    text = _enumValues.GetValue(i).ToString(),
                    style = {
                        width = new StyleLength(StyleKeyword.Auto),
                    }
                };

                if (i > 0) {
                    _buttons[i].style.marginLeft = 0;
                    _buttons[i].style.borderTopLeftRadius = 0;
                    _buttons[i].style.borderBottomLeftRadius = 0;
                    _buttons[i].style.borderLeftWidth = 0;
                }
                
                if (i < _enumValues.Length-1) {
                    _buttons[i].style.marginRight = 0;
                    _buttons[i].style.borderTopRightRadius = 0;
                    _buttons[i].style.borderBottomRightRadius = 0;
                }

                _indexByToggleButton.Add(_buttons[i], i);
                Add(_buttons[i]);
            }
            
            SetValueWithoutNotify(Array.IndexOf(_enumValues, value));
        }

        private void ToggleButtonChangeEvent(ChangeEvent<bool> evt) {
            value = _indexByToggleButton[(ToggleButton) evt.target];
        }

        public void SetValueWithoutNotify(int newValue) {
            _value = newValue;
            TEnumType enumValue = (TEnumType) _enumValues.GetValue(_value);
            for (int i = 0; i < _buttons.Length; i++) {
                _buttons[i].SetValueWithoutNotify(_value == i);
            }
        }

        public int value {
            get => _value;
            set {
                if (value >= _enumValues.Length) return;
                if (value < 0) return;
                if (value == _value) return;
                int previous = _value;
                SetValueWithoutNotify(value);
                
                TEnumType previousEnumValue = (TEnumType) _enumValues.GetValue(previous);
                TEnumType enumValue = (TEnumType) _enumValues.GetValue(value);
                
                using (var evt = ChangeEvent<int>.GetPooled(previous, value)) {
                    evt.target = this;
                    SendEvent(evt);
                }
                
                using (var evt = ChangeEvent<TEnumType>.GetPooled(previousEnumValue, enumValue)) {
                    evt.target = this;
                    SendEvent(evt);
                }
            }
        }
    }
}