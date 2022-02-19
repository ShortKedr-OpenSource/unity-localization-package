# Unity Localization #

## System ##
1. Compiler preprocessor defines ``KRUGAMES_UNITY_LOCALIZATION``. 
   This define serve to external solutions 

## Resource files ##
1. Automatic resource files creation
1. Resource files validation

## User Interface ##
1. New Reworked Editor
1. Workflow: ``languages listed in term``

## OOP Model ##
1. Base ``LocaleTerm`` type and it's inheritance
1. Custom main editor drawers for ``LocaleTerm`` inheritors
1. ``LocaleTerm`` inheritor types for: ``string``, ``Sprite``
1. Ability to extend ``LocaleTerm`` types by user's own type and add it to editor

## Game API ##
1. Native linkers: ``UI/Text``, `TMP_Text`, ``Image``
1. Ability to get term value from concrete language
1. Ability to get term value of corresponding type
1. Ability to change language

## Editor API ##
1. Ability to manage LocaleTerms
1. Ability to inline Localization property editor to any UnityEditor UI 
1. Language Duplicate

## Design of ``LocaleTerm`` type ##
1. Description ``only-in-editor`` property for improving game-dev process

## Programming stuff ##
1. Add to every unity component element ``AddComponentMenu`` attribute, 
   to list it in the user's menu
   

## Linkers ##
Linkers are ``MonoBehaviour`` based components or mechanism that
allow to connect some component to Localization System  

All Linkers must be derived from ``Linker`` base abstract class.  
``Linker`` class can be derived from ``ILinker`` interface if its needed

## Native Linkers ##
Native Linkers are components that
allow to quickly connect one of the default
UnityEngine components with Localization System.  

All Native Linkers must be derived from ``NativeLinker`` base abstract class,
that derived from ``Linker`` class

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

All Plugin Linkers must be derived from ``PluginLinker`` base
class, that derived from ``Linker`` class.  

Plugin Linkers must have preprocessor define directive
(surrounded with ``#ifdef`` and ``#endif``),
such as ``KRUGAMES_UI_SYSTEM``.  
This approach will allow to
include special Plugin Linkers to builds, if
current project has this system in-use

## Custom Linkers ##
Custom linkers are components that allow to quickly connect
game-end logic code to Localization System

All Custom Linkers are must be derived from ``CustomLinker`` base class,
that derived from ``Linker`` class

### Preprocessor directive sub-system ###
Preprocessor define for each group of Plugin Linkers
requires sub-system that will identify connection with
project assemblies.  
Sub-system will add right define directives to compiler,
if assemblies or classes exists.

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