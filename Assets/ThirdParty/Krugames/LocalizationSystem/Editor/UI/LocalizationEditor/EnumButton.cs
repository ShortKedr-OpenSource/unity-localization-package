using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor {
    public class EnumButton<TEnumType> : Box, INotifyValueChanged<TEnumType>, IBindable where TEnumType : Enum {

        private ToggleButton[] _buttons;

        public EnumButton(TEnumType value) {
            this.value = value;
        }

        public EnumButton(SerializedProperty property) {
            this.BindProperty(property);
        }

        public void SetValueWithoutNotify(TEnumType newValue) {
            throw new NotImplementedException();
        }

        public TEnumType value { get; set; }
        public IBinding binding { get; set; }
        public string bindingPath { get; set; }
    }
}