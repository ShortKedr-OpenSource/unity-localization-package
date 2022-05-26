namespace Krugames.LocalizationSystem.Models.Validation.Errors {
    public class TermDuplicateError : ValidationError{
        public TermDuplicateError(LocaleTerm term) 
            : base($"{term.Term} have duplicates") {
        }
    }
}