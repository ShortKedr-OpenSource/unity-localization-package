namespace Krugames.LocalizationSystem.Editor.Package {
    /// <summary>
    /// Provides global Localization package variables
    /// </summary>
    internal static class PackageVariables {

        public static readonly string PackagePath;

        static PackageVariables() {
            PackagePath = PackagePathDefiner.GetPackagePath();
        }
    }
}