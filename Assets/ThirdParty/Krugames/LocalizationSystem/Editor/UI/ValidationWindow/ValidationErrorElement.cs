using Krugames.LocalizationSystem.Models.Validation;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.ValidationWindow {
    public class ValidationErrorElement : HelpBox {

        private const string LabelClassName = nameof(ValidationErrorElement) + "_Label";
        
        private ValidationError _error;

        public ValidationError Error => _error;

        public ValidationErrorElement(ValidationError error) : base(error.Message, HelpBoxMessageType.Error) {
            _error = error;
            this.Q<Label>().AddToClassList(LabelClassName);
        }
    }
}