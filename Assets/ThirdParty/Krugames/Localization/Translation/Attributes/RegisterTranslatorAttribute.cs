using System;

namespace Krugames.LocalizationSystem.Translation.Attributes {

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class RegisterTranslatorAttribute : Attribute {
        
        
        
        public RegisterTranslatorAttribute() {
            
        }
    }
}