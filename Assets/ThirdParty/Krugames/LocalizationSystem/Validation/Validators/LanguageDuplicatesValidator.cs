using System.Collections.Generic;
using System.Linq;
using Krugames.LocalizationSystem.Models.Validation.Errors;
using UnityEngine;

namespace Krugames.LocalizationSystem.Models.Validation.Validators {
    public class LanguageDuplicatesValidator : Validator<Locale[]> {
        public override ValidationReport<Locale[]> ValidateWithReport(Locale[] validationSubject) {

            List<ValidationError> errors = new List<ValidationError>(16);
            HashSet<SystemLanguage> errorLanguages = new HashSet<SystemLanguage>();
            
            for (int i = 0; i < validationSubject.Length; i++) {
                for (int j = 1; j < validationSubject.Length; j++) {
                    if (validationSubject[i] == validationSubject[j]) continue;
                    SystemLanguage first = validationSubject[i].Language;
                    SystemLanguage second = validationSubject[j].Language;
                    if (first == second && !errorLanguages.Contains(first)) {
                        errors.Add(new SystemLanguageDuplicateError(first));
                        errorLanguages.Add(first);
                    }
                }
            }

            return new ValidationReport<Locale[]>(validationSubject, errors.ToArray());
        }
    }
}