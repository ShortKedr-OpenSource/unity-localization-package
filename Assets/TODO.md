# TODOs #

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
1. Ability to extend ``LocaleTerm`` typesв by user's own type and add it to editor

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

### Preprocessor directive sub-system ###
Preprocessor define for each group of Plugin Linkers
requires sub-system that will identify connection with
project assemblies.  
Sub-system will add right define directives to compiler,
if assemblies or classes exists.