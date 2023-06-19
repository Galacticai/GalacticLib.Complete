using GalacticLib.Objects;
using System.Globalization;
using System.Reflection;

namespace GalacticLib.Languages.ISO;

/// <summary> Tools for <see cref="LanguageCode"/> (and others related to it) </summary>
public static class LanguageCodeTools {
    public enum LanguageType {
        /// <summary> Does not inherit a macro language </summary>
        Standalone,
        /// <summary> Inherit a macro language </summary>
        Child,
        /// <summary> Has child languages under it </summary>
        Macro
    }

    /// <summary> Converts <see cref="CultureInfo.CurrentCulture"/>.ThreeLetterISOLanguageName to <see cref="LanguageCode"/> </summary>
    public static LanguageCode Current
        => CultureInfo.CurrentCulture.ThreeLetterISOLanguageName.Parse<LanguageCode>();

    public static TInfoAttr? GetInfoAttribute<TInfoAttr>(this LanguageCode code) where TInfoAttr : LanguageInfoAttribute
        => code.GetFieldInfo().GetCustomAttribute<TInfoAttr>(false);
    public static bool IsMacro(this LanguageCode code)
        => code.GetInfoAttribute<MacroLanguageInfoAttribute>() != null;
    public static bool IsChild(this LanguageCode code)
        => code.GetInfoAttribute<ChildLanguageInfoAttribute>() != null;
    public static bool IsStandalone(this LanguageCode code)
        => code.GetInfoAttribute<LanguageInfoAttribute>() != null;
    public static LanguageType GetLanguageType(this LanguageCode code) {
        if (code.IsMacro()) return LanguageType.Macro;
        else if (code.IsChild()) return LanguageType.Child;
        else if (code.IsStandalone()) return LanguageType.Standalone;
        else throw new($"The language code (\"{code}\") is missing {nameof(LanguageInfoAttribute)}. Please contact the developer.");
    }

    public static MacroLanguageName? ToMacroName(this LanguageCode code)
        => code.IsMacro() ? (MacroLanguageName)code : null;
    public static MacroLanguageCode? ToMacroCode(this LanguageCode code)
        => code.IsMacro() ? (MacroLanguageCode)code : null;

    public static MacroLanguageName ToMacroName(this MacroLanguageCode macroCode)
        => (MacroLanguageName)macroCode;
    public static LanguageName ToName(this MacroLanguageName name)
        => (LanguageName)name;
    public static LanguageCode ToDistinctCode(this MacroLanguageName name)
        => (LanguageCode)name;
    public static LanguageCode ToDistinctCode(this MacroLanguageCode code)
        => (LanguageCode)code;
    public static LanguageCode ToDistinctCode(this MacroLanguageCode2 code2)
        => (LanguageCode)code2;
    public static LanguageCode ToDistinctCode(this MacroLanguageCode3 code3)
        => (LanguageCode)code3;

    public static string Description(this LanguageCode code)
        => code.Info().Description;
    public static string Description(this MacroLanguageName macroName)
        => macroName.ToDistinctCode().Description();
    public static LanguageCode[] ChildLanguages(this MacroLanguageName macroName)
        => ((MacroLanguageInfoAttribute)macroName.ToDistinctCode().Info()).ChildrenCodes;

    public static LanguageInfoAttribute Info(this LanguageCode code)
        => code.GetLanguageType() switch { //!? Cant be null because GetLanguageType already checks
            LanguageType.Macro => code.GetInfoAttribute<MacroLanguageInfoAttribute>()!,
            LanguageType.Child => code.GetInfoAttribute<ChildLanguageInfoAttribute>()!,
            _ => code.GetInfoAttribute<LanguageInfoAttribute>()!
        };

}
