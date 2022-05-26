using System.Collections.Generic;
using Krugames.LocalizationSystem.Models.Terms;
using Krugames.LocalizationSystem.Models.Validation.Errors;

namespace Krugames.LocalizationSystem.Models.Validation.Validators {
    public class LocaleEmptyFieldValidator : Validator<Locale> {
        public override ValidationReport<Locale> ValidateWithReport(Locale validationSubject) {
            LocaleTerm[] terms = validationSubject.GetTerms();
            
            List<ValidationError> errors = new List<ValidationError>(16);

            for (int i = 0; i < terms.Length; i++) {
                if (terms[i] is StringTerm stringTerm) {
                    if (string.IsNullOrEmpty(stringTerm.SmartValue)) 
                        errors.Add(new StringTermEmptyFieldValueError(stringTerm));
                } else {
                    if (terms[i].Value == null) 
                        errors.Add(new LocaleTermNullFieldValueError(terms[i]));
                }
            }
            
            return new ValidationReport<Locale>(validationSubject, errors.ToArray());
        }
    }
}