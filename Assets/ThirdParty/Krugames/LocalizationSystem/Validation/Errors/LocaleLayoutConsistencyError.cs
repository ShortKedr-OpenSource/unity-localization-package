namespace Krugames.LocalizationSystem.Models.Validation.Errors {
    public class LocaleLayoutConsistencyError : ValidationError {
        public LocaleLayoutConsistencyError(Locale layoutLead, Locale layoutFollower) : 
            base($"{layoutFollower.name} layout does not match lead {layoutLead.name} layout! " +
                 $"Use locale's properties window to fix that") { //TODO remove this properties message part
        }
    }
}