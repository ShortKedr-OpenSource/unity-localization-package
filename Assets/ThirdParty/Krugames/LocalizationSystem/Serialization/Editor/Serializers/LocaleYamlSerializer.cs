using Krugames.LocalizationSystem.Editor.Serialization.Attributes;
using Krugames.LocalizationSystem.Editor.Serialization.DataTransferObjects;
using Krugames.LocalizationSystem.Editor.Serialization.Serializers;
using Krugames.LocalizationSystem.Models.Interfaces;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

[assembly: RegisterLocaleSerializer(typeof(LocaleYamlSerializer), "YAML", "yaml")]

namespace Krugames.LocalizationSystem.Editor.Serialization.Serializers {
    
    /// <summary>
    /// ILocale to YAML Serializer
    /// </summary>
    public class LocaleYamlSerializer : LocaleSerializer<string>{
        public override string SerializeSmart(ILocale locale) {
            LocaleData localeData = new LocaleData(locale);
            ISerializer serializer = new SerializerBuilder().
                WithNamingConvention(CamelCaseNamingConvention.Instance).
                Build();
            string yaml = serializer.Serialize(localeData);
            return yaml;
        }

        public override void DeserializeSmart(ILocale targetLocale, string data) {
            throw new System.NotImplementedException();
        }
    }
}