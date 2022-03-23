using System;
using Krugames.LocalizationSystem.Models.Interfaces;

namespace Krugames.LocalizationSystem.Editor.Serialization {

    public abstract class LocaleSerializer {
        public abstract object Serialize(ILocale locale);
        
        public abstract void Deserialize(ILocale targetLocale, object data);
    }
    
    public abstract class LocaleSerializer<TEndDataType> : LocaleSerializer {
        public override object Serialize(ILocale locale) {
            return SerializeSmart(locale);
        }

        public override void Deserialize(ILocale targetLocale, object data) {
            if (data is TEndDataType implData) {
                DeserializeSmart(targetLocale, implData);
            } else {
                throw new ArgumentException("Wrong import data! Type mismatch");
            }
        }

        public abstract TEndDataType SerializeSmart(ILocale locale);
        public abstract void DeserializeSmart(ILocale targetLocale, TEndDataType data);
    }
}