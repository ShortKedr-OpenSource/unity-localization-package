namespace Krugames.LocalizationSystem.Models.Validation {

    public abstract class Validator {
        public abstract ValidationReport ValidateWithReport(object validationSubject);
        
        public bool Validate(object validationSubject) {
            return ValidateWithReport(validationSubject).IsTargetValid;
        }
    }
    
    public abstract class Validator<TValidationType> : Validator {
        public abstract ValidationReport<TValidationType> ValidateWithReport(TValidationType validationSubject);
        
        public bool Validate(TValidationType validationSubject) {
            return ValidateWithReport(validationSubject).IsTargetValid;
        }

        public override ValidationReport ValidateWithReport(object target) {
            if (target is TValidationType validationSubject) {
                return ValidateWithReport(validationSubject);
            }
            return new ValidationReport(target, new ValidationError[] {
                new ValidationError("Validation subject type is invalid!"),
            });
        }
    }
}