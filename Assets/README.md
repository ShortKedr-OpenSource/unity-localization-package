# Unity Localization Package #

## System ##
1. Package automatically defines `KRUGAMES_UNITY_LOCALIZATION` keyword. 
   This keyword serve to be used in external solutions, outside of package

## Resource files ##
1. Automatic resource files creation: `Locale`, `LocaleTerm`.
1. Resource files validation: `Duplets check`.

## User Interface ##
1. New reworked Localization Editor 
   with `languages listed int term` workflow approach.
2. Enhanced `Locale` inspector.

## OOP Model ##
1. Base `LocaleTerm` type and it's inheritance
1. Custom main editor drawers for `LocaleTerm` inheritors
1. `LocaleTerm` inheritor types for: `string`, `Sprite`,
1. Ability to extend `LocaleTerm` types by user's own type and add it to editor

## Game API ##
1. Native linkers: `UI/Text`, `TMP_Text`, `Image`
2. Ability to get term value from concrete language
3. Ability to get term value of corresponding type
4. Ability to change language
5. TODO extend this list

## Editor API ##
1. Ability to manage LocaleTerms
2. Ability to inline Localization property editor to any UnityEditor UI 
3. Language Duplicate
4. TODO extend this list (utilities etc)

## Design of `LocaleTerm` type ##
1. Description `only-in-editor` property for improving game-dev process

## Programming stuff ##
1. Add to every unity component element `AddComponentMenu` attribute, 
   to list it in the user's menu

## Linkers ##
Linkers are `MonoBehaviour` based components or mechanism that
allow to connect some component to Localization System  

All Linkers must be derived from `Linker` base abstract class.  
`Linker` class can be derived from `ILinker` interface if its needed

## Native Linkers ##
Native Linkers are components that
allow to quickly connect one of the default
UnityEngine components with Localization System.  

All Native Linkers must be derived from `NativeLinker` base abstract class,
that derived from `Linker` class

There is list of native component types, whose Native Linkers must
be included to base version:
1. `Text`
1. `TMP_Text`
1. `SpriteRenderer`
1. `Image`
1. `RawImage`
1. `AudioSource`

Other native components can have Native Linkers as well

## Plugin Linkers ##
Plugin Linkers are components that allow to quickly connect
not UnityEngine components with Localization System.  

All Plugin Linkers must be derived from `PluginLinker` base
class, that derived from `Linker` class.  

Plugin Linkers must have preprocessor define directive
(surrounded with `#ifdef` and `#endif`),
such as `KRUGAMES_UI_SYSTEM`.  
This approach will allow to
include special Plugin Linkers to builds, if
current project has this system in-use

## Custom Linkers ##
Custom linkers are components that allow to quickly connect
game-end logic code to Localization System

All Custom Linkers are must be derived from `CustomLinker` base class,
that derived from `Linker` class

## External Plugins Connection ##
Plugin Connection is way to connect different plugins to 
Localization Package.
This feature can be done in 2 different way: _easy_ and _smart_.  

**Easy way**: integrate each known plugin directly and 
use plugin' defined keywords to manage integration active state

**Smart way**: create Integration API, that will allow
any kind of stuff, plugin of external system integrate with Localization Package

Both ways can be presented in code. 
Easy approach can be used only for known plugins and provide additional features this way.
Smart approach can be used to integrate both known plugins and user plugins, 
but no additional system-close features will be presented.

## Localization Update event ##
`LocalizationUpdate` - event, that happens after current
localization was somehow updated in runtime.  
Can be used to tell Linkers and other systems that content needs
to be updated to propper content

## Enhanced User Experience ##
This part describes features that will
help enhance user experience.

### Term Selector ###
UI dropdown term selector with search included
or window with list of terms and search.  
UI tool that will help select and type terms to linkers
and custom solutions.

### Odin Integration ###
Odin integration for all included engine side components.
Integration must not be persistent and enabled only if
special compiler define is presented - `ODIN_INSPECTOR_3`.  
Not persistent approach will allow to use system across
multiple projects and teams, some of them will not use Odin,
some of them will use it

## Dynamic API ##
By default `Locale` and `LocaleTerm` are static instances,
they are stored and loaded once into LocalizationPackage.
They can not be rebuild or updated during runtime.

Dynamic API solves this _static problem_ by allowing adding
new locales during runtime. These locales are not `Locale`
instances. There is special `DynamicLocale` type for this.

API can be extended by `DynamicLocaleProvider` base type, that provide
unified way to provide `DynamicLocale` from some source.
This feature still in experimental state

## Serialization ##
Editor only feature that allow Serialize `Locale` instance
to different target formats.

Any new serializer type must be inherited from `LocaleSerializer` base type.  
Localization Package can be extended by user' serializer.  

Any serializer should have `RegisterLocaleSerializer` attribute.
Serializer will work correctly with whole system this way.

List of supported serialize formats:
1. CSV - _import, export, read, modify_
2. JSON - _import, export, read, modify_
3. XML - _import, export, read, modify_
4. YAML - _import, export, read, modify_

## Translation ##
TODO

