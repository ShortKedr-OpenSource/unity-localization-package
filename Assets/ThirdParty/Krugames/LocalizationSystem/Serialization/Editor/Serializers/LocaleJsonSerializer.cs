using System.Collections.Generic;
using Krugames.LocalizationSystem.Editor.Serialization.Attributes;
using Krugames.LocalizationSystem.Editor.Serialization.DataTransferObjects;
using Krugames.LocalizationSystem.Editor.Serialization.Serializers;
using Krugames.LocalizationSystem.Models.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[assembly: RegisterLocaleSerializer(typeof(LocaleJsonSerializer), "JSON", "json")]

namespace Krugames.LocalizationSystem.Editor.Serialization.Serializers {
    
    /// <summary>
    /// ILocale to JSON Serializer
    /// </summary>
    public class LocaleJsonSerializer : LocaleSerializer<string> {

        private JsonSerializerSettings _settings;
        private Formatting _formatting;

        public LocaleJsonSerializer() : this(Formatting.None){
        }

        public LocaleJsonSerializer(Formatting formatting = Formatting.None, JsonSerializerSettings settings = null) {
            _settings = settings;
            _formatting = formatting;
            if (_settings == null) {
                _settings = new JsonSerializerSettings() {
                    Converters = new List<JsonConverter>() {
                        new StringEnumConverter(),
                    },
                };
            }
        }

        public override string SerializeSmart(ILocale locale) {
            LocaleData localeData = new LocaleData(locale);
            return JsonConvert.SerializeObject(localeData, _formatting, _settings);
        }

        public override void DeserializeSmart(IModifiableLocale targetLocale, string data) {
            LocaleData localeData = JsonConvert.DeserializeObject<LocaleData>(data, _settings);
            localeData.SetDataToLocale(targetLocale);
        }
    }
}