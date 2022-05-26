using System.Collections.Generic;
using System.Linq;
using Krugames.LocalizationSystem.Models.Structs;
using Krugames.LocalizationSystem.Models.Validation.Errors;

namespace Krugames.LocalizationSystem.Models.Validation.Validators {
    public class LayoutConsistencyValidator : Validator<LayoutConsistencyValidator.LocaleConsistencyModel>{

        public class LocaleConsistencyModel {
            public readonly Locale Lead;
            public readonly Locale[] Followers;

            public LocaleConsistencyModel(Locale lead, Locale[] followers) {
                this.Lead = lead;
                this.Followers = followers;
            }
        }
        
        public override ValidationReport<LocaleConsistencyModel> ValidateWithReport(LocaleConsistencyModel validationSubject) {
            
            List<ValidationError> errors = new List<ValidationError>(16);

            TermStructureInfo[] leadLayout = validationSubject.Lead.GetLayout();
            List<TermStructureInfo[]> followerLayouts = new List<TermStructureInfo[]>(validationSubject.Followers.Length);
            for (int i = 0; i < validationSubject.Followers.Length; i++) {
                followerLayouts.Add(validationSubject.Followers[i].GetLayout());
            }

            for (int i = 0; i < followerLayouts.Count; i++) {
                if (validationSubject.Lead == validationSubject.Followers[i]) continue;
                if (!CheckLayoutConsistency(leadLayout, followerLayouts[i])) {
                    errors.Add(new LocaleLayoutConsistencyError(
                        validationSubject.Lead, validationSubject.Followers[i]));
                }
            }


            return new ValidationReport<LocaleConsistencyModel>(validationSubject, errors.ToArray());
        }

        public bool CheckLayoutConsistency(TermStructureInfo[] first, TermStructureInfo[] second) {
            if (first.Length != second.Length) return false;

            HashSet<string> firstTermsCache = new HashSet<string>();
            for (int i = 0; i < first.Length; i++) {
                if (firstTermsCache.Contains(first[i].termName)) return false;
                firstTermsCache.Add(first[i].termName);
            }

            HashSet<string> secondTermsCache = new HashSet<string>();
            for (int i = 0; i < second.Length; i++) {
                if (!firstTermsCache.Contains(second[i].termName)) return false;
                if (secondTermsCache.Contains(second[i].termName)) return false;
                secondTermsCache.Add(second[i].termName);
            }

            return true;
            //TODO add deeper report errors, show inconsistent parts;
        }
    }
}