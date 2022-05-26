namespace Krugames.LocalizationSystem.Models.Validation.Errors {
    public class LocaleTermNullFieldValueError : ValidationError {
        public LocaleTermNullFieldValueError(LocaleTerm term) 
            : base($"{term.Term} has null field value") {
        }
    }
}