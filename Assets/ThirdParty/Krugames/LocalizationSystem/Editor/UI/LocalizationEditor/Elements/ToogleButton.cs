using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor {
    public class ToggleButton : Button, INotifyValueChanged<bool> {
        public enum ToggleMode {
            Switchable = 0,
            Pickable = 1,
        }
        
        private const string CheckedClassName = "Checked";

        private bool _value;
        private ToggleMode _mode;

        public ToggleButton(bool value, EventCallback<ChangeEvent<bool>> changeCallback, ToggleMode mode = ToggleMode.Switchable) {
            RegisterCallback(changeCallback);
            this.value = value;
            _mode = mode;
            clickable.clicked += ClickEvent;
        }

        private void ClickEvent() {
            switch (_mode) {
                case ToggleMode.Switchable:
                    value = !value;
                    break;
                case ToggleMode.Pickable:
                    value = true;
                    break;
            }
        }


        public void SetValueWithoutNotify(bool newValue) {
            _value = newValue;
            RemoveFromClassList(CheckedClassName);
            if (_value) AddToClassList(CheckedClassName);
        }

        public bool value {
            get => _value;
            set {
                if (_value == value) return;
                bool previous = _value;
                SetValueWithoutNotify(value);

                using (var evt = ChangeEvent<bool>.GetPooled(previous, value)) {
                    evt.target = this;
                    SendEvent(evt);
                }
            }
        }
    }
}