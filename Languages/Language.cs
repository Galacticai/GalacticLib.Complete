namespace GalacticLib.Languages;
public static class Language {
    public static LanguageName ToILanguages(this string language)
        => SystemLanguage(language);
    public static string ToGoogleLanguageID(this LanguageName language)
        => GoogleLanguageID(language);

    /// <summary> Current .NET system language <see cref="string"/> code in 2 or 3 letters </summary>
    public static string TwoLetterISOLanguageName
        => System.Globalization.CultureInfo.InstalledUICulture.TwoLetterISOLanguageName;
    #region Shortcuts
    public static bool CurrentLangIsEnglish => TwoLetterISOLanguageName == "en";
    public static bool CurrentLangIsArabic => TwoLetterISOLanguageName == "ar";
    public static bool CurrentLangIsChinese => TwoLetterISOLanguageName == "zh";
    #endregion


    ///// <summary> Translate a string using Google Translate API </summary>
    ///// <param name="input"> <see cref="string"/> to be translated </param>
    ///// <param name="toLanguage"> Destination language </param>
    ///// <returns> Translated <see cref="string"/> + <see cref="out"/> Detected original language</returns>
    //public static string Translate(string input, ILanguages toLanguage) {
    //    TranslationClient translationclient = TranslationClient.Create();
    //    TranslationResult translationResult
    //            = translationclient.TranslateText(input, ILanguagesToGoogleLanguageCodes(toLanguage));
    //    return translationResult.TranslatedText;
    //}
    [Obsolete("This is not implimented yet")]
    public static async Task<string> TranslateText(string input, LanguageName toLanguage = LanguageName.System) {
        //? Find current system language if not provided
        if (toLanguage == LanguageName.System)
            toLanguage = SystemLanguage();
        //? Cancel if English to English
        if (toLanguage == LanguageName.English)
            return input;

        throw new NotImplementedException();


        string url = string.Format($"https://translate.google.com/?text={input}&tl={toLanguage.ToGoogleLanguageID()}");
        string translatedString = await new HttpClient().GetStringAsync(url);

        translatedString = translatedString[(translatedString.IndexOf("<span title=\"") + "<span title=\"".Length)..];
        translatedString = translatedString[(translatedString.IndexOf(">") + 1)..];
        translatedString = translatedString[..translatedString.IndexOf("</span>")];
        translatedString = translatedString.Trim();
        return translatedString;
    }

    /// <summary> Convert .NET language code <see cref="string"/> to <see cref="LanguageName"/> readable by ScreenFIRE </summary>
    /// <param name="language"> Language to convert || Will use system language if null </param>
    /// <returns> ScreenFIRE <see cref="LanguageName"/> corresponding to the provided <paramref name="language"/></returns>
    public static LanguageName SystemLanguage(string? language = null)
        => (language ?? TwoLetterISOLanguageName) switch {
            //?
            //? Commented = not supported by Google Translate API
            //?
            //// "iv" => ILanguages.InvariantLanguage_InvariantCountry,
            "af" => LanguageName.Afrikaans,
            "am" => LanguageName.Amharic,
            "ar" => LanguageName.Arabic,
            //"as" => ILanguages.Assamese,
            "az" => LanguageName.Azerbaijani,
            "be" => LanguageName.Belarusian,
            "bg" => LanguageName.Bulgarian,
            //"bn" => ILanguages.Bangla,
            //"bo" => ILanguages.Tibetan,
            //"br" => ILanguages.Breton,
            "bs" => LanguageName.Bosnian,
            "ca" => LanguageName.Catalan,
            //"chr" => ILanguages.Cherokee,
            "cs" => LanguageName.Czech,
            "cy" => LanguageName.Welsh,
            "da" => LanguageName.Danish,
            "de" => LanguageName.German,
            //"dsb" => ILanguages.LowerSorbian,
            //"dz" => ILanguages.Dzongkha_Bhutan,
            "el" => LanguageName.Greek,
            "en" => LanguageName.English,
            "es" => LanguageName.Spanish,
            "et" => LanguageName.Estonian,
            "eu" => LanguageName.Basque,
            "fa" => LanguageName.Persian,
            //"ff" => ILanguages.Fulah,
            "fi" => LanguageName.Finnish,
            "fil" => LanguageName.Filipino,
            //"fo" => ILanguages.Faroese,
            "fr" => LanguageName.French,
            //"fy" => ILanguages.WesternFrisian,
            "ga" => LanguageName.Irish,
            //"gd" => ILanguages.ScottishGaelic,
            "gl" => LanguageName.Galician,
            //"gsw" => ILanguages.SwissGerman,
            "gu" => LanguageName.Gujarati,
            "ha" => LanguageName.Hausa,
            "haw" => LanguageName.Hawaiian,
            "hi" => LanguageName.Hindi,
            "hr" => LanguageName.Croatian,
            //"hsb" => ILanguages.UpperSorbian,
            "hu" => LanguageName.Hungarian,
            "hy" => LanguageName.Armenian,
            "id" => LanguageName.Indonesian,
            "ig" => LanguageName.Igbo,
            //"ii" => ILanguages.SichuanYi,
            "is" => LanguageName.Icelandic,
            "it" => LanguageName.Italian,
            "ja" => LanguageName.Japanese,
            "ka" => LanguageName.Georgian,
            "kk" => LanguageName.Kazakh,
            //"kl" => ILanguages.Kalaallisut,
            "km" => LanguageName.Khmer,
            "kn" => LanguageName.Kannada,
            "ko" => LanguageName.Korean,
            //"kok" => ILanguages.Konkani,
            //"ks" => ILanguages.Kashmiri,
            //"ku" => ILanguages.Kurdish,
            "ky" => LanguageName.Kyrgyz,
            "lb" => LanguageName.Luxembourgish,
            "lo" => LanguageName.Lao,
            "lt" => LanguageName.Lithuanian,
            "lv" => LanguageName.Latvian,
            "mi" => LanguageName.Maori,
            "mk" => LanguageName.Macedonian,
            "ml" => LanguageName.Malayalam,
            "mn" => LanguageName.Mongolian,
            "mr" => LanguageName.Marathi,
            "ms" => LanguageName.Malay,
            "mt" => LanguageName.Maltese,
            //"my" => ILanguages.Burmese,
            //"nb" => ILanguages.NorwegianBokmål,
            "ne" => LanguageName.Nepali,
            "nl" => LanguageName.Dutch,
            //"nn" => ILanguages.NorwegianNynorsk,
            //"om" => ILanguages.Oromo,
            //"or" => ILanguages.Odia,
            "pa" => LanguageName.Punjabi,
            "pl" => LanguageName.Polish,
            "ps" => LanguageName.Pashto,
            "pt" => LanguageName.Portuguese,
            //"rm" => ILanguages.Romansh,
            "ro" => LanguageName.Romanian,
            "ru" => LanguageName.Russian,
            //"rw" => ILanguages.Kinyarwanda,
            //"sah" => ILanguages.Sakha,
            "sd" => LanguageName.Sindhi,
            //"se" => ILanguages.NorthernSami,
            "si" => LanguageName.Sinhala,
            "sk" => LanguageName.Slovak,
            "sl" => LanguageName.Slovenian,
            //"smn" => ILanguages.InariSami,
            "so" => LanguageName.Somali,
            "sq" => LanguageName.Albanian,
            "sr" => LanguageName.Serbian,
            "sv" => LanguageName.Swedish,
            "sw" => LanguageName.Swahili,
            "ta" => LanguageName.Tamil,
            "te" => LanguageName.Telugu,
            "tg" => LanguageName.Tajik,
            "th" => LanguageName.Thai,
            //"ti" => ILanguages.Tigrinya,
            //"tk" => ILanguages.Turkmen,
            "tr" => LanguageName.Turkish,
            //"tt" => ILanguages.Tatar,
            //"tzm" => ILanguages.CentralAtlasTamazight,
            //"ug" => ILanguages.Uyghur,
            "uk" => LanguageName.Ukrainian,
            "ur" => LanguageName.Urdu,
            "uz" => LanguageName.Uzbek,
            "vi" => LanguageName.Vietnamese,
            //"wo" => ILanguages.Wolof,
            "xh" => LanguageName.Xhosa,
            "yi" => LanguageName.Yiddish,
            "yo" => LanguageName.Yoruba,
            //"zh" => ILanguages.Chinese,
            "zh" => LanguageName.ChineseSimplified,
            "zu" => LanguageName.Zulu,


            _ => LanguageName.Other
        };


    /// <summary> Convert <see cref="LanguageName"/> to <see cref="string"/> readable by Google Translate API </summary>
    /// <param name="language"> Language to convert </param>
    /// <returns> Google Translate language code <see cref="string"/> corresponding to the provided <paramref name="language"/></returns>
    public static string GoogleLanguageID(LanguageName language = LanguageName.System)
        => (language == LanguageName.System ? SystemLanguage() : language) switch {
            LanguageName.Afrikaans => "af",
            LanguageName.Albanian => "sq",
            LanguageName.Amharic => "am",
            LanguageName.Arabic => "ar",
            LanguageName.Armenian => "hy",
            LanguageName.Azerbaijani => "az",
            LanguageName.Basque => "eu",
            LanguageName.Belarusian => "be",
            LanguageName.Bengali => "bn",
            LanguageName.Bosnian => "bs",
            LanguageName.Bulgarian => "bg",
            LanguageName.Catalan => "ca",
            LanguageName.Cebuano => "ceb",
            LanguageName.Chichewa => "ny",
            LanguageName.ChineseSimplified => "zh",
            LanguageName.ChineseTraditional => "zh-TW",
            LanguageName.Corsican => "co",
            LanguageName.Croatian => "hr",
            LanguageName.Czech => "cs",
            LanguageName.Danish => "da",
            LanguageName.Dutch => "nl",
            LanguageName.English => "en",
            LanguageName.Esperanto => "eo",
            LanguageName.Estonian => "et",
            LanguageName.Filipino => "tl",
            LanguageName.Finnish => "fi",
            LanguageName.French => "fr",
            LanguageName.Frisian => "fy",
            LanguageName.Galician => "gl",
            LanguageName.Georgian => "ka",
            LanguageName.German => "de",
            LanguageName.Greek => "el",
            LanguageName.Gujarati => "gu",
            LanguageName.HaitianCreole => "ht",
            LanguageName.Hausa => "ha",
            LanguageName.Hawaiian => "haw",
            LanguageName.Hindi => "hi",
            LanguageName.Hmong => "hmn",
            LanguageName.Hungarian => "hu",
            LanguageName.Icelandic => "is",
            LanguageName.Igbo => "ig",
            LanguageName.Indonesian => "id",
            LanguageName.Irish => "ga",
            LanguageName.Italian => "it",
            LanguageName.Japanese => "ja",
            LanguageName.Javanese => "jw",
            LanguageName.Kannada => "kn",
            LanguageName.Kazakh => "kk",
            LanguageName.Khmer => "km",
            LanguageName.Korean => "ko",
            LanguageName.KurdishKurmanji => "ku",
            LanguageName.Kyrgyz => "ky",
            LanguageName.Lao => "lo",
            LanguageName.Latin => "la",
            LanguageName.Latvian => "lv",
            LanguageName.Lithuanian => "lt",
            LanguageName.Luxembourgish => "lb",
            LanguageName.Macedonian => "mk",
            LanguageName.Malagasy => "mg",
            LanguageName.Malay => "ms",
            LanguageName.Malayalam => "ml",
            LanguageName.Maltese => "mt",
            LanguageName.Maori => "mi",
            LanguageName.Marathi => "mr",
            LanguageName.Mongolian => "mn",
            LanguageName.MyanmarBurmese => "my",
            LanguageName.Nepali => "ne",
            LanguageName.Norwegian => "no",
            LanguageName.Pashto => "ps",
            LanguageName.Persian => "fa",
            LanguageName.Polish => "pl",
            LanguageName.Portuguese => "pt",
            LanguageName.Punjabi => "pa",
            LanguageName.Romanian => "ro",
            LanguageName.Russian => "ru",
            LanguageName.Samoan => "sm",
            LanguageName.ScotsGaelic => "gd",
            LanguageName.Serbian => "sr",
            LanguageName.Sesotho => "st",
            LanguageName.Shona => "sn",
            LanguageName.Sindhi => "sd",
            LanguageName.Sinhala => "si",
            LanguageName.Slovak => "sk",
            LanguageName.Slovenian => "sl",
            LanguageName.Somali => "so",
            LanguageName.Spanish => "es",
            LanguageName.Sundanese => "su",
            LanguageName.Swahili => "sw",
            LanguageName.Swedish => "sv",
            LanguageName.Tajik => "tg",
            LanguageName.Tamil => "ta",
            LanguageName.Telugu => "te",
            LanguageName.Thai => "th",
            LanguageName.Turkish => "tr",
            LanguageName.Ukrainian => "uk",
            LanguageName.Urdu => "ur",
            LanguageName.Uzbek => "uz",
            LanguageName.Vietnamese => "vi",
            LanguageName.Welsh => "cy",
            LanguageName.Xhosa => "xh",
            LanguageName.Yiddish => "yi",
            LanguageName.Yoruba => "yo",
            LanguageName.Zulu => "zu",

            //? Other
            _ => "en"
        };
}

