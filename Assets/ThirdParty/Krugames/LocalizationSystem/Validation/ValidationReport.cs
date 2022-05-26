using System;
using Krugames.LocalizationSystem.Models.Interfaces;

namespace Krugames.LocalizationSystem.Models.Validation {

    public class ValidationReport {

        public readonly object Target;
        public readonly ValidationError[] Errors;

        public bool IsTargetValid => Errors.Length == 0;

        public ValidationReport(object target, ValidationError[] errors) {
            Target = target;
            Errors = errors;
        }
    }
    
    public class ValidationReport<TValidationType> : ValidationReport {

        public readonly TValidationType SmartTarget;

        public ValidationReport(TValidationType target, ValidationError[] errors) : base(target, errors) {
            SmartTarget = target;
        }
    }
}