using System;

namespace Krugames.LocalizationSystem.Models.Attributes {
    
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class RegisterTermAttribute : Attribute {

        public readonly static Type BaseType = typeof(LocaleTerm<>);
        
        public readonly Type TermType;
        public readonly string Name;

        public RegisterTermAttribute(Type termType, string name = "") {
            TermType = termType;
            Name = (name == "") ? TermType.Name : name;
        }

        public bool IsValid => TermType != null && TermType.IsAssignableFrom(BaseType);

    }
    
}