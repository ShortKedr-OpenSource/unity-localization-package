//TODO Add types: warning, error, etc;
namespace Krugames.LocalizationSystem.Models.Validation {

    public class ValidationError {
        
        public readonly string Message;

        public ValidationError(string message) {
            Message = message;
        }
    }

    //TODO review
    /*public class ValidationError<TValidationReportType> : ValidationError
        where TValidationReportType : ValidationReport {
        public ValidationError(string message) : base(message) {
        }
    }*/
}