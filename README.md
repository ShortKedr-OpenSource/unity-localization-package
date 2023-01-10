# Unity Localization Package #
by ShortKedr  
First working version of project is ready.  
Most of project features still under development  

This page still under development  

Localization Package source files are placed by next path: `/Assets/ThirdParty/Krugames/LocalizationSystem`


## Licensing ##
You can't use this solution for commercial purposes.  
For more information, please, contact me on email `shortkedr@gmail.com`


## Main Localization Editor ##
To open editor select menu `Window -> Krugames -> Localization`.  
Editor looks as showed on following images:  

First look:
![Localization Editor First Look](/Docs/LocalizationEditor_1.png)  

Managing locale:
![Localization Editor Managing Locale](/Docs/LocalizationEditor_2.png) 

Functions right panel:
![Localization Editor Functions Panel](/Docs/LocalizationEditor_3.png)  

Validation report:
![Localization Editor Validation Report](/Docs/LocalizationEditor_4.png)  


## Default Linkers ##
Linkers allow to make logic-less localization bind.    
There are following linkers currently available:  

TMP_Text Linker (text):
![TMP_Text Linker](/Docs/Linker_1.png) 

Image Linker (sprite):
![Image Linker](/Docs/Linker_2.png)  

Raw Image Linker (texture):
![Raw Image Linker](/Docs/Linker_3.png)  

Audio Source Linker (audio clip):
![Audio Source Linker](/Docs/Linker_4.png)


## Project-wide settings ##
Package has it's own project-wide settings. 
There will be more setting points in future.

![LocalizationProjectSettings](/Docs/LocalizationProjectSetting_1.png)

## Localizatio Example Scene ##
Project has example scene, that uses default linkers.
Launch `Assets/Examples/SimpleExample/SimpleExample.unity`

![LocalizationProjectSettings](/Docs/LocalizationExmapleScene.png)


## Basic API ##
```csharp
// Get current used language
SystemLanguage currentLanguage = Localization.CurrentLanguage;

// Get current locale for current language
ILocale currentLocale = Localization.CurrentLocale;

// Get all dynamic locales
DynamicLocale[] dynamicLocales = Localization.DynamicLocales;

// Get list of all supported languages
SystemLanguage[] supportedLanguage = Localization.SupportedLanguages;

// Get list of validated locales 
ILocale[] validLocales = Localization.ValidLocales;

// Get base validated locale
ILocale baseValidLocale = Localization.BaseValidLocale;

// Initialize localization system manually
Localization.Initialize();

// Get locale by language
ILocale locale = Localization.GetLocale(SystemLanguage.English);

// Set current language
Localization.SetLanguage(SystemLanguage.Russian);

// Checks if localization system contains specified language
bool hasLanguage = Localization.SupportsLanguage(SystemLanguage.Swedish);

// Add new dynamic locale to system
DynamicLocale dynamicLocale = new DynamicLocale();
Localization.AddDynamicLocale(dynamicLocale);

// Remove specified dynamic locale
Localization.RemoveDynamicLocale(dynamicLocale);

// Unload all unused asset resources
Localization.UnloadUnusedResources();

// Get untyped term value
object gValue = Localization.GetTermValue("term_name");

// Get untyped term value, but guarantee it has specified type 
object tgValue = Localization.GetTermValue("term_name", typeof(string));

// Get term value of specified type
string value = Localization.GetTermValue<string>("term_name");

// Get untyped term value for specified language
object glValue = Localization.GetTermValue("term_name", SystemLanguage.English);

// Get untyped term value for specified language, but gurantee it has specified type
object tglValue = Localization.GetTermValue("term_name", typeof(string), SystemLanguage.English);

// Get term value of specified type from specified language
string lValue = Localization.GetTermValue<string>("term_name", SystemLanguage.English);

// Setup language change callback
void Callback() {
    Console.WriteLine("Language was changed");
};
Localization.AddLanguageUpdateCallback(Callback);

//Setup complex language change callback
void ComplexCallback(LocaleLibrary library, SystemLanguage oldLang, SystemLanguage newLang) {
    Console.WriteLine($"Language was changed from{oldLang} to {newLang} in {library}");
};
Localization.AddLanguageUpdateCallback(ComplexCallback);

// Remove language change callback
Localization.RemoveLanguageUpdateCallback(Callback);

// Remove complext language change callback
Localization.RemoveLanguageUpdateCallback(ComplexCallback);
```

## Available Extensions ##

There some possible system extensions that currently available.

### Custom Locale Term ###
```csharp
[assembly: RegisterLocaleTerm(typeof(PersonTerm), "Person Data")]

public class PersonTerm : LocaleTerm<PersonData> {
    
}

[Serializable]
public class PersonData {
    public string firstName;
    public string secondName;
    public Gender gender;
    public int age;

    public override string ToString() {
        return $"{nameof(PersonData)} ({firstName} {secondName}, {gender}, {age})";
    }
}

public enum Gender {
    Male = 0,
    Female = 1,
}
```

### Custom Translator ###
```csharp
[assembly: RegisterTranslator(typeof(MyCustomTranslator))]

public class MyCustomTranslator : StringTranslator {
    public override void Translate(string data, SystemLanguage from, SystemLanguage to, TranslationSuccessDelegate successCallback,
        TranslationFailDelegate failCallback) {

        bool success = Random.Range(0, 100) > 50;
        if (success) successCallback?.Invoke("This is translated text", to, from);
        else failCallback?.Invoke(data, from, to);
    }
}
```

### Custom serializer ###
```csharp
[assembly: RegisterLocaleSerializer(typeof(FunnySerializer), "Funny File", "funny")]

public class FunnySerializer : LocaleSerializer<string> {
    public override string SerializeSmart(ILocale locale) {
        return $"{locale.Language}\n" +
               $"This is serialized data";
    }

    public override void DeserializeSmart(IModifiableLocale targetLocale, string data) {
        Console.WriteLine("No serialization for Funny Files");
    }
}
```

### Custom Validator (limited) ###
```csharp
public class MyValidator : Validator<Locale> {
    public override ValidationReport<Locale> ValidateWithReport(Locale validationSubject) {
        List<ValidationError> errors = new List<ValidationError>(16);
        errors.Add(new ValidationError("This is error example"));
        errors.Add(new ValidationError("Another error example"));
        
        ValidationReport<Locale> report = new ValidationReport<Locale>(validationSubject, errors.ToArray());
        return report;
    }
}

public class ValidationExpansionExample {

    [MenuItem("Examples/Validation")]
    private static void LaunchReport() {
        Locale locale = (Locale)LocaleLibrary.Instance.BaseLocale;
        
        MyValidator validator = new MyValidator();
        ValidationReport report = validator.ValidateWithReport(locale);
        ValidationReportWindow.ShowReport(report, $"Validation for {locale.name}");
    }
}
```

### Custom Linker ###
```csharp
public class ComplexText : CustomLinker {
    
    [SerializeField] private string descriptionTerm = "none";
    [SerializeField] private string silenceWordTerm;
    [SerializeField] private TMP_Text text;

    [SerializeField] private float damage = 90f;
    [SerializeField] private float tickInterval = 1f;
    [SerializeField] private float skillTime = 1f;

    public override void UpdateContent() {
        string desc = Localization.GetTermValue<string>(descriptionTerm);
        string silenceWord = Localization.GetTermValue<string>(silenceWordTerm);
        text.text = desc
            .Replace("@silence_word@", silenceWord)
            .Replace("@damage@", damage.ToString("0"))
            .Replace("@tick_interval@", tickInterval.ToString("0"))
            .Replace("@skill_time@", skillTime.ToString("0"));
    }
}
```
