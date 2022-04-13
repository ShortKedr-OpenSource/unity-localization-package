using System.Collections.Generic;
using Krugames.LocalizationSystem.Editor.Serialization.Attributes;
using Krugames.LocalizationSystem.Editor.Serialization.DataTransferObjects;
using Krugames.LocalizationSystem.Editor.Serialization.Serializers;
using Krugames.LocalizationSystem.Models.Interfaces;
using Newtonsoft.Json;

[assembly: RegisterLocaleSerializer(typeof(LocaleJsonSerializer), "JSON")]

namespace Krugames.LocalizationSystem.Editor.Serialization.Serializers {
    
    /// <summary>
    /// ILocale to JSON Serializer
    /// </summary>
    public class LocaleJsonSerializer : LocaleSerializer<string> {

        private JsonSerializerSettings _settings;
        private Formatting _formatting;
        
        public LocaleJsonSerializer(Formatting formatting = Formatting.None, JsonSerializerSettings settings = null) {
            _settings = settings;
            _formatting = formatting;
        }

        public override string SerializeSmart(ILocale locale) {
            LocaleData localeData = new LocaleData(locale);
            return JsonConvert.SerializeObject(localeData, _formatting, _settings);
        }

        public override void DeserializeSmart(ILocale targetLocale, string data) {
            List<TermData> terms = JsonConvert.DeserializeObject<List<TermData>>(data, _settings);
            //TODO change layout of locale;
            //TODO assign value;
            //TODO assign assets from Database;
        }
    }
}