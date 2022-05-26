using UnityEngine;

namespace Krugames.LocalizationSystem.Models.Validation.Errors {
    public class SystemLanguageDuplicateError : ValidationError {
        public SystemLanguageDuplicateError(SystemLanguage duplicatedLanguage)
            : base($"{duplicatedLanguage} language have duplicates") {
        }
    }
}