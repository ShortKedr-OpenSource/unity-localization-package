using System;
using System.Collections.Generic;
using System.Reflection;
using Krugames.LocalizationSystem.Models.Attributes;

namespace Krugames.LocalizationSystem.Models.Locators {
    public static class LocaleTermLocator {

        private const int DefaultLocatorBuffer = 32;

        public static RegisterLocaleTermAttribute[] LocaleTermLocators {
            get {
                List<RegisterLocaleTermAttribute> locators = new List<RegisterLocaleTermAttribute>(DefaultLocatorBuffer);

                foreach (ICustomAttributeProvider assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                    var attributes = assembly.GetCustomAttributes(typeof(RegisterLocaleTermAttribute), false);

                    for (int i = 0; i < attributes.Length; i++) {
                        if (attributes[i] is RegisterLocaleTermAttribute registrator) {
                            if (!registrator.IsValid) continue;
                            locators.Add(registrator);
                        }
                    }
                }

                return locators.ToArray();
            }
        }
    }
}