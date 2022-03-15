using System;

namespace Krugames.LocalizationSystem.Translation.Attributes {

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class RegisterTranslatorAttribute : Attribute {
        public static readonly Type BaseType = typeof(Translator<>);

        /// <summary>
        /// Translator type, used to register translator in system
        /// Can not be abstract, must be inherited from inheritor of Translator<TDataType>
        /// to work as intended 
        /// </summary>
        public readonly Type TranslatorType;

        /// <summary>
        /// Base translator type, used to register translator groups. 
        /// Should be abstract and inherited from Translator<TDataType> to work as intended
        /// </summary>
        public readonly Type BaseTranslatorType;

        public RegisterTranslatorAttribute(Type translatorType) {
            TranslatorType = translatorType;
            BaseTranslatorType = typeof(Translator);
        }

        public RegisterTranslatorAttribute(Type translatorType, Type baseTranslatorType) {
            TranslatorType = translatorType;
            BaseTranslatorType = baseTranslatorType;
        }

        public bool IsValid => BaseType.GetGenericTypeDefinition().IsAssignableFrom(TranslatorType); //TODO change
    }
}