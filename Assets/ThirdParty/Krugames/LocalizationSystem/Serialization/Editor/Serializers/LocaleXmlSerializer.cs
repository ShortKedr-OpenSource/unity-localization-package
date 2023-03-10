using System.IO;
using System.Xml.Serialization;
using Krugames.LocalizationSystem.Editor.Serialization.Attributes;
using Krugames.LocalizationSystem.Editor.Serialization.DataTransferObjects;
using Krugames.LocalizationSystem.Editor.Serialization.Serializers;
using Krugames.LocalizationSystem.Models.Interfaces;

[assembly: RegisterLocaleSerializer(typeof(LocaleXmlSerializer), "XML", "xml")]

namespace Krugames.LocalizationSystem.Editor.Serialization.Serializers {
    
    /// <summary>
    /// ILocale to XML Serializer
    /// </summary>
    public class LocaleXmlSerializer : LocaleSerializer<string> {
        public override string SerializeSmart(ILocale locale) {
            LocaleData localeData = new LocaleData(locale);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LocaleData));
            using StringWriter stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, localeData);
            return stringWriter.ToString();
        }

        public override void DeserializeSmart(IModifiableLocale targetLocale, string data) {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LocaleData));
            using StringReader stringReader = new StringReader(data);
            LocaleData localeData = (LocaleData) xmlSerializer.Deserialize(stringReader);
            localeData.SetDataToLocale(targetLocale);
        }
    }
}