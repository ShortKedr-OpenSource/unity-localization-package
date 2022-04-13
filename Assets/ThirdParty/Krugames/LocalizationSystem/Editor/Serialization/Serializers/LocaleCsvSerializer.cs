using System.Text;
using Krugames.LocalizationSystem.Editor.Serialization.Attributes;
using Krugames.LocalizationSystem.Editor.Serialization.DataTransferObjects;
using Krugames.LocalizationSystem.Editor.Serialization.Serializers;
using Krugames.LocalizationSystem.Models.Interfaces;

[assembly: RegisterLocaleSerializer(typeof(LocaleCsvSerializer), "CSV")]

namespace Krugames.LocalizationSystem.Editor.Serialization.Serializers {

    /// <summary>
    /// ILocale to CSV Serializer
    /// </summary>
    public class LocaleCsvSerializer : LocaleSerializer<string>{
        public override string SerializeSmart(ILocale locale) {
            LocaleData localeData = new LocaleData(locale);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Language;{locale.Language}");
            builder.AppendLine("");
            builder.AppendLine($"Term;Value;ValueType;IsAsset");
            for (int i = 0; i < localeData.termData.Length; i++) {
                builder.AppendLine($"{localeData.termData[i].term};" +
                                   $"{localeData.termData[i].value};" +
                                   $"{localeData.termData[i].valueType};" +
                                   $"{localeData.termData[i].isAsset}");
            }
            return builder.ToString();
        }

        public override void DeserializeSmart(ILocale targetLocale, string data) {
            throw new System.NotImplementedException();
        }
    }
}