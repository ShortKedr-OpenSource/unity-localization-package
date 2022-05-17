using System;
using Krugames.LocalizationSystem.Models.Interfaces;

namespace Krugames.LocalizationSystem.Models.Validation {
    public static class LocaleValidator {

        public static bool Validate(ILocale locale) {
            return ValidateWithReport(locale).IsValid;
        }

        public static LocaleValidatingReport ValidateWithReport(ILocale locale) {
            throw new NotImplementedException();
        }
    }
}