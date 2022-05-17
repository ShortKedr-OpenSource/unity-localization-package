using System;
using Krugames.LocalizationSystem.Models.Interfaces;

namespace Krugames.LocalizationSystem.Editor.Serialization {

    /// <summary>
    /// Base class for locale serializer.
    /// Serializes ILocale to data
    /// Deserializes data to ILocale
    /// </summary>
    public abstract class LocaleSerializer {
        public abstract object Serialize(ILocale locale);
        
        public abstract void Deserialize(ILocale targetLocale, object data);
        
    }
    
    /// <summary>
    /// Generic base class for locale serializer
    /// </summary>
    /// <typeparam name="TEndDataType">Type of serialized data</typeparam>
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