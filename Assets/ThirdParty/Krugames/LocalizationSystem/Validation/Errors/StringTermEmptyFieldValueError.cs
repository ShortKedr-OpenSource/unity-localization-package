using Krugames.LocalizationSystem.Models.Terms;

namespace Krugames.LocalizationSystem.Models.Validation.Errors {
    public class StringTermEmptyFieldValueError : ValidationError {
        public StringTermEmptyFieldValueError(StringTerm stringTerm) 
            : base($"{stringTerm.Term} string is null or empty") {
        }
    }
}