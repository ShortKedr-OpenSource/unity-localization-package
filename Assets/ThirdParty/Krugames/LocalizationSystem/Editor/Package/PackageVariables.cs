namespace Krugames.LocalizationSystem.Editor.Package {
    /// <summary>
    /// Provides global Localization package variables
    /// </summary>
    internal static class PackageVariables {

        public const string DefineSymbol = "KRUGAMES_UNITY_LOCALIZATION";
        
        public static readonly string PackagePath;

        static PackageVariables() {
            PackagePath = PackagePathDefiner.GetPackagePath();
        }
    }
}