using System;
using Krugames.LocalizationSystem.Models.Interfaces;

namespace Krugames.LocalizationSystem.Models.Validation {
    public class LocaleValidatingReport {

        public readonly ILocale Target;
        public readonly LocaleValidatingError[] Errors;
        public bool IsValid => Errors.Length == 0;

        public LocaleValidatingReport(ILocale target, LocaleValidatingError[] errors) {
            Target = target;
            Errors = errors ?? Array.Empty<LocaleValidatingError>();
        }
    }
}