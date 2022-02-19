namespace Krugames.LocalizationSystem.Linkers {
    /// <summary>
    /// Tells developer that current linker is plugin linker.
    /// Plugin Linkers must have preprocessor define directive, surrounded with #ifdef and #endif, such as KRUGAMES_UI_SYSTEM.
    /// This approach allow to include special plugin linkers to builds, if current project has this system in-use
    /// Plugin Linkers always links things of plugin system to localization system.
    /// Do not use inheritance from this class for other purposes!*** to keep understanding clear.s
    /// </summary>
    public abstract class PluginLinker : Linker {
    }
}