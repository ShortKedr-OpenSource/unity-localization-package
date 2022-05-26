using System.Collections.Generic;
using System.Linq;
using Krugames.LocalizationSystem.Models.Validation.Errors;
using UnityEngine;

namespace Krugames.LocalizationSystem.Models.Validation.Validators {
    public class TermDuplicateValidator : Validator<Locale> {
        public override ValidationReport<Locale> ValidateWithReport(Locale validationSubject) {
            LocaleTerm[] terms = validationSubject.GetTerms();
            
            List<ValidationError> errors = new List<ValidationError>(16);
            HashSet<string> errorTerms = new HashSet<string>();

            for (int i = 0; i < terms.Length; i++) {
                for (int j = 1; j < terms.Length; j++) {
                    if (terms[i] == terms[j]) continue;
                    if (terms[i].Term == terms[j].Term && !errorTerms.Contains(terms[i].Term)) {
                        errors.Add(new TermDuplicateError(terms[i]));
                        errorTerms.Add(terms[i].Term);
                    }
                }
            }

            return new ValidationReport<Locale>(validationSubject, errors.ToArray());
        }
    }
}